using System;
using libtcodWrapper;

namespace TCODDemo
{

    class TCODDemo : IDisposable
    {
        TCODConsole sampleConsole;

        const int SAMPLE_SCREEN_WIDTH = 46;
        const int SAMPLE_SCREEN_HEIGHT = 20;

        const int SAMPLE_SCREEN_X = 20;
        const int SAMPLE_SCREEN_Y = 10;

        delegate void SampleRender(bool first, TCOD_key key);

        struct sample
        {
            public sample(string n, SampleRender r)
            {
                name = n;
                render = r;
            }
            public string name;
            public SampleRender render;
        }

        public TCODDemo()
        {
            sampleConsole = new TCODConsoleRoot(SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, "TCOD Demo", false);
        }

        private bool ArgsRemaining(string[] args, int pos, int numberRequested)
        {
            return (pos + numberRequested < args.Length);
        }

        TCODColor[] render_cols;
        int[] render_dirr;
        int[] render_dirg;
        int[] render_dirb;
        TCODRandom random;
        void render_colors(bool first, TCOD_key key)
        {
            int TOPLEFT = 0;
            int TOPRIGHT = 1;
            int BOTTOMLEFT = 2;
            int BOTTOMRIGHT = 3;

            TCODColor textColor = new TCODColor();

            /* ==== slighty modify the corner colors ==== */
            if (first)
            {
                TCODSystem.SetFPS(0);
                sampleConsole.Clear();
            }
            /* ==== slighty modify the corner colors ==== */
            for (int c = 0; c < 4; c++)
            {
                /* move each corner color */
                int component = random.GetRandomInt(0, 2);
                switch (component)
                {
                    case 0:
                        render_cols[c].r += (byte)(5 * render_dirr[c]);
                        if (render_cols[c].r == 255)
                            render_dirr[c] = -1;
                        else if (render_cols[c].r == 0)
                            render_dirr[c] = 1;
                        break;
                    case 1:
                        render_cols[c].g += (byte)(5 * render_dirg[c]);
                        if (render_cols[c].g == 255)
                            render_dirg[c] = -1;
                        else if (render_cols[c].g == 0)
                            render_dirg[c] = 1;
                        break;
                    case 2:
                        render_cols[c].b += (byte)(5 * render_dirb[c]);
                        if (render_cols[c].b == 255)
                            render_dirb[c] = -1;
                        else if (render_cols[c].b == 0)
                            render_dirb[c] = 1;
                        break;
                }
            }

            /* ==== scan the whole screen, interpolating corner colors ==== */
            for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
            {
                float xcoef = (float)(x) / (SAMPLE_SCREEN_WIDTH - 1);
                /* get the current column top and bottom colors */
                TCODColor top = TCODColor.Interpolate(render_cols[TOPLEFT], render_cols[TOPRIGHT], xcoef);
                TCODColor bottom = TCODColor.Interpolate(render_cols[BOTTOMLEFT], render_cols[BOTTOMRIGHT], xcoef);
                for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                {
                    float ycoef = (float)(y) / (SAMPLE_SCREEN_HEIGHT - 1);
                    /* get the current cell color */
                    TCODColor curColor = TCODColor.Interpolate(top, bottom, ycoef);
                    sampleConsole.SetCharBackground(x, y, curColor, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
                }
            }

            /* ==== print the text ==== */
            /* get the background color at the text position */
            textColor = sampleConsole.GetCharBackground(SAMPLE_SCREEN_WIDTH / 2, 5);
            /* and invert it */
            textColor.r = (byte)(255 - textColor.r);
            textColor.g = (byte)(255 - textColor.g);
            textColor.b = (byte)(255 - textColor.b);
            sampleConsole.SetForegroundColor(textColor);
            /* the background behind the text is slightly darkened using the BKGND_MULTIPLY flag */
            sampleConsole.SetBackgroundColor(TCODColor.TCOD_grey);
            sampleConsole.PrintLineRect("The Doryen library uses 24 bits colors, for both background and foreground.", SAMPLE_SCREEN_WIDTH / 2, 5, SAMPLE_SCREEN_WIDTH - 2, SAMPLE_SCREEN_HEIGHT - 1, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_MULTIPLY), TCODLineAlign.Center);

            /* put random text (for performance tests) */
            for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
            {
                for (int y = 12; y < SAMPLE_SCREEN_HEIGHT; y++)
                {
                    TCODColor col = sampleConsole.GetCharBackground(x, y);
                    col = TCODColor.Interpolate(col, TCODColor.TCOD_black, 0.5f);
                    int c = random.GetRandomInt(System.Convert.ToByte('a'), System.Convert.ToByte('z'));
                    sampleConsole.SetForegroundColor(col);
                    sampleConsole.PutChar(x, y, (char)c);
                }
            }
        }


        TCODConsole off_secondary;
        TCODConsole off_screenshot;
        byte off_alpha = 0; // alpha value for the blit operation
        bool off_init = false; // draw the secondary screen only the first time
        int off_counter = 0;
        int off_x = 0, off_y = 0; // secondary screen position
        int off_xdir = 1, off_ydir = 1; // movement direction
        void render_offscreen(bool first, TCOD_key key)
        {
            if (!off_init)
            {
                off_init = true;
                off_secondary.DrawBox(0, 0, SAMPLE_SCREEN_WIDTH / 2, SAMPLE_SCREEN_HEIGHT / 2, false, "Offscreen console");
                off_secondary.PrintLineRect("You can render to an offscreen console and blit in on another one, simulating alpha transparency.", SAMPLE_SCREEN_WIDTH / 4, 2, SAMPLE_SCREEN_WIDTH / 2 - 2, SAMPLE_SCREEN_HEIGHT / 2, TCODLineAlign.Center);
            }
            if (first)
            {
                TCODSystem.SetFPS(30); // fps limited to 30
                // get a "screenshot" of the current sample screen
                sampleConsole.Blit(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, off_screenshot, 0, 0, 255);

            }
            off_counter++;
            if (off_counter % 20 == 0)
            {
                // move the secondary screen every 2 seconds
                off_x += off_xdir;
                off_y += off_ydir;
                if (off_x == SAMPLE_SCREEN_WIDTH / 2)
                    off_xdir = -1;
                else if (off_x == 0)
                    off_xdir = 1;
                if (off_y == SAMPLE_SCREEN_HEIGHT / 2)
                    off_ydir = -1;
                else if (off_y == 0)
                    off_ydir = 1;
            }
            off_alpha = (byte)(255 * (1.0f + Math.Cos(TCODSystem.GetElapsedSeconds() * 2.0f)) / 2.0f);

            // restore the initial screen
            off_screenshot.Blit(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, sampleConsole, 0, 0, 255);

            // blit the overlapping screen
            off_secondary.Blit(0, 0, SAMPLE_SCREEN_WIDTH / 2, SAMPLE_SCREEN_HEIGHT / 2, sampleConsole, off_x, off_y, off_alpha);
        }

        static TCODConsole line_bk;
        static TCODBackground line_bkFlag = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET);
        static bool line_init = false;
        void render_lines(bool first, TCOD_key key)
        {
            sampleConsole.Clear();
            if ( key.vk == TCOD_keycode.TCODK_ENTER || key.vk == TCOD_keycode.TCODK_KPENTER ) 
            {
                // switch to the next blending mode
                if ( line_bkFlag.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_ALPH)
                    line_bkFlag = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_NONE);
                else
                    line_bkFlag++;
            }
            if (line_bkFlag.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_ALPH) 
            {
                // for the alpha mode, update alpha every frame
                double alpha = (1.0f+Math.Cos(TCODSystem.GetElapsedSeconds()*2))/2.0f;
                line_bkFlag = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_ALPH, alpha);
            }
            else if (line_bkFlag.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_ADDA) 
            {
                // for the add alpha mode, update alpha every frame
                double alpha = (1.0f + Math.Cos(TCODSystem.GetElapsedSeconds() * 2)) / 2.0f;
                line_bkFlag = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_ADDA, alpha);
            }

            if (!line_init)
            {
                // initialize the colored background
                for (int x=0; x < SAMPLE_SCREEN_WIDTH; x ++) 
                {
                    for (int y=0; y < SAMPLE_SCREEN_HEIGHT; y++) 
                    {
                        TCODColor col = new TCODColor();
                        col.r = (byte)(x* 255 / (SAMPLE_SCREEN_WIDTH-1));
                        col.g = (byte)((x+y)* 255 / (SAMPLE_SCREEN_WIDTH-1+SAMPLE_SCREEN_HEIGHT-1));
                        col.b = (byte)(y* 255 / (SAMPLE_SCREEN_HEIGHT-1));
                        line_bk.SetCharBackground(x, y, col, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
                    }
                }
                line_init = true;
            }
            if ( first )
            {
                TCODSystem.SetFPS(30); // fps limited to 30
                sampleConsole.SetForegroundColor(TCODColor.TCOD_white);
            }
            
            // blit the background
            line_bk.Blit(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, sampleConsole, 0, 0, 255);
                
               
            
            // render the gradient
            int recty = (int)((SAMPLE_SCREEN_HEIGHT - 2) * ((1.0f + Math.Cos(TCODSystem.GetElapsedSeconds())) / 2.0f));
            for (int x = 0 ; x < SAMPLE_SCREEN_WIDTH; x++) 
            {
                TCODColor col = new TCODColor();
                col.r = (byte)(x*255/SAMPLE_SCREEN_WIDTH);
                col.g = (byte)(x * 255 / SAMPLE_SCREEN_WIDTH);
                col.b = (byte)(x * 255 / SAMPLE_SCREEN_WIDTH);
                sampleConsole.SetCharBackground(x, recty, col, line_bkFlag);
                sampleConsole.SetCharBackground(x, recty+1, col, line_bkFlag);
                sampleConsole.SetCharBackground(x, recty+2, col, line_bkFlag);
            }

            // calculate the segment ends
            float angle = TCODSystem.GetElapsedSeconds()*2.0f;
            float cosAngle = (float)Math.Cos(angle);
            float sinAngle = (float)Math.Sin(angle);
            int xo = (int)(SAMPLE_SCREEN_WIDTH/2*(1 + cosAngle));
            int yo = (int)(SAMPLE_SCREEN_HEIGHT/2 + sinAngle * SAMPLE_SCREEN_WIDTH/2);
            int xd = (int)(SAMPLE_SCREEN_WIDTH/2*(1 - cosAngle));
            int yd = (int)(SAMPLE_SCREEN_HEIGHT/2 - sinAngle * SAMPLE_SCREEN_WIDTH/2);

            // render the line
            int xx=xo,yy=yo;
            TCODLineDrawing.InitLine(xx,yy,xd,yd);
            do
            {
                if (xx >= 0 && yy >= 0 && xx < SAMPLE_SCREEN_WIDTH && yy < SAMPLE_SCREEN_HEIGHT)
                {
                    sampleConsole.SetCharBackground(xx, yy, TCODColor.TCOD_light_blue, line_bkFlag);
                }
            }
            while (!TCODLineDrawing.StepLine(ref xx, ref yy));

            // print the current flag
            sampleConsole.PrintLine(line_bkFlag.GetBackgroundFlag().ToString() + " (ENTER to change)", 2, 2, TCODLineAlign.Left);
        }

        enum noiseFunctions { Noise, FBM, Turbulence };
        static string [] noise_funcName = 
        {
            "1 : perlin noise",
		    "2 : fbm         ",
		    "3 : turbulence  "
        };
        noiseFunctions noise_func = noiseFunctions.Noise;
        TCODNoise noise;
        float noise_dx = 0.0f, noise_dy = 0.0f;
        float noise_octaves = 4.0f;
        float noise_hurst = TCODNoise.NoiseDefaultHurst;
        float noise_lacunarity = TCODNoise.NoiseDefaultLacunarity;
        float noise_zoom = 3.0f;
        void render_noise(bool first, TCOD_key key)
        {
            if ( first ) 
            {
                TCODSystem.SetFPS(30); /* limited to 30 fps */
            }
            sampleConsole.Clear();
    
            /* texture animation */
            noise_dx += 0.01f;
            noise_dy += 0.01f;
            
            /* render the 2d noise function */
            for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++ ) 
            {
                for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++ ) 
                {
                    float [] f = new float[2];
                    float value = 0.0f;
                    byte c;
                    TCODColor col = new TCODColor();
                    f[0] = noise_zoom * x / SAMPLE_SCREEN_WIDTH + noise_dx;
                    f[1] = noise_zoom * y / SAMPLE_SCREEN_HEIGHT + noise_dy;
        
                    switch (noise_func) 
                    {
                        case noiseFunctions.Noise:
                            value = noise.GetPerlinNoise(f); 
                            break;
                        case noiseFunctions.FBM:
                            value = noise.GetBrownianMotion(f, noise_octaves);
                            break;
                        case noiseFunctions.Turbulence:
                            value = noise.GetTurbulence(f, noise_octaves);
                            break;
                    }
        
                    c = (byte)((value+1.0f)/2.0f*255);
                    /* use a bluish color */
                    col.r = (byte)(c / 2);
                    col.g = (byte)(c / 2);
                    col.b = c;
                    sampleConsole.SetCharBackground(x, y, col, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
                }
            }
            
            /* draw a transparent rectangle */
            sampleConsole.SetBackgroundColor(TCODColor.TCOD_grey);
            sampleConsole.DrawRect(2, 2, (noise_func == noiseFunctions.Noise ? 16 : 24), (noise_func == noiseFunctions.Noise ? 4 : 7), false, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_MULTIPLY));

            /* draw the text */
            for (noiseFunctions curfunc = noiseFunctions.Noise; curfunc <= noiseFunctions.Turbulence; curfunc++) 
            {
                if (curfunc == noise_func) 
                {
                    sampleConsole.SetForegroundColor(TCODColor.TCOD_white);
                    sampleConsole.SetBackgroundColor(TCODColor.TCOD_light_blue);
                    sampleConsole.PrintLine(noise_funcName[(int)curfunc], 2, 2 + (int)(curfunc), new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET), TCODLineAlign.Left);
                }
                else
                {
                    sampleConsole.SetForegroundColor(TCODColor.TCOD_grey);
                    sampleConsole.PrintLine(noise_funcName[(int)curfunc], 2, 2 + (int)(curfunc), new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_NONE), TCODLineAlign.Left);
                }
            }
            /* draw parameters */
            sampleConsole.SetForegroundColor(TCODColor.TCOD_white);
            sampleConsole.PrintLine("Y/H : zome (" + noise_zoom.ToString("0.0") + ")", 2, 5, TCODLineAlign.Left);

            if (noise_func != noiseFunctions.Noise) 
            {
                sampleConsole.PrintLine("E/D : hurst (" + noise_hurst.ToString("0.0") + ")", 2, 6, TCODLineAlign.Left);
                sampleConsole.PrintLine("R/F : lacunarity (" + noise_lacunarity.ToString("0.0") + ")", 2, 7, TCODLineAlign.Left);
                sampleConsole.PrintLine("T/G : octaves (" + noise_octaves.ToString("0.0") + ")", 2, 8, TCODLineAlign.Left);
            }

            /* handle keypress */
            if (key.vk == TCOD_keycode.TCODK_NONE)
                return;
            if (key.c >= '1' && key.c <= '3')
            {
                noise_func = (noiseFunctions)(key.c - '1');
            }
            else if ( key.c == 'E' || key.c == 'e' ) 
            {
                /* increase hurst */
                noise_hurst+=0.1f;
                noise.Dispose();
                noise = new TCODNoise(2, noise_hurst, noise_lacunarity);
            }
            else if ( key.c == 'D' || key.c == 'd' ) 
            {
                /* decrease hurst */
                noise_hurst-=0.1f;
                noise.Dispose();
                noise = new TCODNoise(2, noise_hurst, noise_lacunarity);
            }
            else if ( key.c == 'R' || key.c == 'r' ) 
            {
                /* increase lacunarity */
                noise_lacunarity+=0.5f;
                noise.Dispose();
                noise = new TCODNoise(2, noise_hurst, noise_lacunarity);
            }
            else if ( key.c == 'F' || key.c == 'f' ) 
            {
                /* decrease lacunarity */
                noise_lacunarity -= 0.5f;
                noise.Dispose();
                noise = new TCODNoise(2, noise_hurst, noise_lacunarity);
            }
            else if (key.c == 'T' || key.c == 't')
            {
                /* increase octaves */
                noise_octaves += 0.5f;
            }
            else if ( key.c == 'G' || key.c == 'g' ) 
            {
                /* decrease octaves */
                noise_octaves -= 0.5f;
            }
            else if ( key.c == 'Y' || key.c == 'y' ) 
            {
                /* increase zoom */
                noise_zoom += 0.2f;
            }
            else if ( key.c == 'H' || key.c == 'h' ) 
            {
                /* decrease zoom */
                noise_zoom -= 0.2f;
            }

        }

        void render_fov(bool first, TCOD_key key)
        {
            sampleConsole.Clear();
        }

        TCODImage img;
        TCODImage circle;
        TCODColor blue = new TCODColor(0, 0, 255);
        TCODColor green = new TCODColor(0, 255, 0);
        uint lastSwitch = 0;
        bool swap = false;
        void render_image(bool first, TCOD_key key)
        {
            sampleConsole.Clear();

            if (img == null)
            {
                img = new TCODImage("skull.bmp");
                circle = new TCODImage("circle.bmp");
            }

            if (first)
                TCODSystem.SetFPS(30);  /* limited to 30 fps */

            sampleConsole.SetBackgroundColor(TCODColor.TCOD_black);
            sampleConsole.Clear();

            float x = SAMPLE_SCREEN_WIDTH / 2 + (float)Math.Cos(TCODSystem.GetElapsedSeconds()) * 10.0f;
            float y = (float)(SAMPLE_SCREEN_HEIGHT/2);
            float scalex = 0.2f + 1.8f * (1.0f + (float)Math.Cos(TCODSystem.GetElapsedSeconds() / 2)) / 2.0f;
            float scaley = scalex;
            float angle = TCODSystem.GetElapsedSeconds();
            uint elapsed = TCODSystem.GetElapsedMilli() / 2000;

            if (elapsed > lastSwitch)
            {
                lastSwitch = elapsed;
                swap = !swap;
            }

            if (swap)
            {  
                /* split the color channels of circle.bmp */
                /* the red channel */
                sampleConsole.SetBackgroundColor(TCODColor.TCOD_red);
                sampleConsole.DrawRect(0, 3, 15, 15, false, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
                circle.BlitRect(sampleConsole, 0, 3, -1, -1, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_MULTIPLY));
                /* the green channel */
                sampleConsole.SetBackgroundColor(green);
                sampleConsole.DrawRect(15, 3, 15, 15, false, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
                circle.BlitRect(sampleConsole, 15, 3, -1, -1, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_MULTIPLY));
                /* the blue channel */
                sampleConsole.SetBackgroundColor(blue);
                sampleConsole.DrawRect(30, 3, 15, 15, false, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
                circle.BlitRect(sampleConsole, 30, 3, -1, -1, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_MULTIPLY));
            }
            else 
            {
                /* render circle.bmp with normal blitting */
                circle.BlitRect(sampleConsole, 0, 3, -1, -1, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
                circle.BlitRect(sampleConsole, 15, 3, -1, -1, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
                circle.BlitRect(sampleConsole, 30, 3, -1, -1, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
            }
           img.Blit(sampleConsole, x, y, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_ADDA, .6), scalex, scaley, angle);
        }

        bool mouse_lbut = false, mouse_rbut = false, mouse_mbut = false;
        void render_mouse(bool first, TCOD_key key)
        {
            if (first)
            {
                sampleConsole.SetBackgroundColor(TCODColor.TCOD_grey);
                sampleConsole.SetForegroundColor(TCODColor.TCOD_light_yellow);
                TCODMouse.MoveMouse(320, 200);
                TCODMouse.ShowCursor(true);
            }

            sampleConsole.Clear();
            TCODMouse mouse = TCODMouse.GetStatus();

            if (mouse.lbutton_pressed)
                mouse_lbut = !mouse_lbut;
            if (mouse.rbutton_pressed)
                mouse_rbut = !mouse_rbut;
            if (mouse.mbutton_pressed)
                mouse_mbut = !mouse_mbut;

            string s1 = "Mouse position : " + mouse.x.ToString("000") + " x " + mouse.y.ToString("000") + "\n";
            string s2 = "Mouse cell     : " + mouse.cx.ToString("000") + " x " + mouse.cy.ToString("000") + "\n";
            string s3 = "Mouse movement : " + mouse.dx.ToString("000") + " x " + mouse.dy.ToString("000") + "\n";
            string s4 = "Left button    : " + (mouse.lbutton ? " ON" : "OFF") + " (toggle " + (mouse_lbut ? " ON" : "OFF") + ")\n";
            string s5 = "Right button   : " + (mouse.rbutton ? " ON" : "OFF") + " (toggle " + (mouse_rbut ? " ON" : "OFF") + ")\n";
            string s6 = "Middle button  : " + (mouse.mbutton ? " ON" : "OFF") + " (toggle " + (mouse_mbut ? " ON" : "OFF") + ")\n";

            string s = s1 + s2 + s3 + s4 + s5 + s6;

            sampleConsole.PrintLine(s, 1, 1, TCODLineAlign.Left);

            sampleConsole.PrintLine("1 : Hide cursor\n2 : Show cursor", 1, 10, TCODLineAlign.Left);
            if (key.c == '1')
                TCODMouse.ShowCursor(false);
            else if (key.c == '2')
                TCODMouse.ShowCursor(true);
        }

        private sample[] sampleList = new sample[7];

        private void fillSampleList()
        {
            sampleList[0] = new sample("  True colors        ", render_colors);
            sampleList[1] = new sample("  Offscreen console  ", render_offscreen);
            sampleList[2] = new sample("  Line drawing       ", render_lines);
            sampleList[3] = new sample("  Perlin noise       ", render_noise);
            sampleList[4] = new sample("  Field of view      ", render_fov);
            sampleList[5] = new sample("  Image toolkit      ", render_image);
            sampleList[6] = new sample("  Mouse support      ", render_mouse);
        }

        private void setupStaticData()
        {
            random = new TCODRandom();

            render_cols = new TCODColor[4];
            render_cols[0] = new TCODColor(50, 40, 150);
            render_cols[1] = new TCODColor(240, 85, 5);
            render_cols[2] = new TCODColor(50, 35, 240);
            render_cols[3] = new TCODColor(10, 200, 130);

            render_dirr = new int[] { 1, -1, 1, 1 };
            render_dirg = new int[] { 1, -1, -1, 1 };
            render_dirb = new int[] { 1, 1, 1, -1 };

            off_secondary = rootConsole.GetNewConsole(SAMPLE_SCREEN_WIDTH / 2, SAMPLE_SCREEN_HEIGHT / 2);
            off_screenshot = rootConsole.GetNewConsole(SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);
            line_bk = rootConsole.GetNewConsole(SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);
            noise = new TCODNoise(2, noise_hurst, noise_lacunarity);
        }

        public void Dispose()
        {
            sampleConsole.Dispose();
            rootConsole.Dispose();
            off_secondary.Dispose();
            off_screenshot.Dispose();
            line_bk.Dispose();
            noise.Dispose();
            random.Dispose();
            if (img != null)
                img.Dispose();
            if (circle != null)
                circle.Dispose();
        }

        TCODConsoleRoot rootConsole;
        public int Run(string[] args)
        {
            fillSampleList();

            int curSample = 0; // index of the current sample
            bool first = true; // first time we render a sample
            TCOD_key key = new TCOD_key();
            string font = "terminal.bmp";
            int charWidth = 8;
            int charHeight = 8;
            int nbCharH = 16;
            int nbCharV = 16;
            int fullscreenWidth = 0;
            int fullscreenHeight = 0;
            bool fullscreen = false;
            bool inRow = false;
            TCODColor keyColor = TCODColor.TCOD_black;

            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] == "-font" && ArgsRemaining(args, i, 1))
                {
                    i++;
                    font = args[i];
                }
                else if (args[i] == "-font-char-size" && ArgsRemaining(args, i, 2))
                {
                    i++;
                    charWidth = System.Convert.ToInt32(args[i]);
                    i++;
                    charHeight = System.Convert.ToInt32(args[i]);
                }
                else if (args[i] == "-font-layout" && ArgsRemaining(args, i, 2))
                {
                    i++;
                    nbCharH = System.Convert.ToInt32(args[i]);
                    i++;
                    nbCharV = System.Convert.ToInt32(args[i]);
                }
                else if (args[i] == "-fullscreen-resolution" && ArgsRemaining(args, i, 2))
                {
                    i++;
                    fullscreenWidth = System.Convert.ToInt32(args[i]);
                    i++;
                    fullscreenHeight = System.Convert.ToInt32(args[i]);
                }
                else if (args[i] == "-fullscreen")
                {
                    fullscreen = true;
                }
                else if (args[i] == "-font-in-row")
                {
                    inRow = true;
                }
                else if (args[i] == "-font-key-color" && ArgsRemaining(args, i, 3))
                {
                    i++;
                    keyColor.r = System.Convert.ToByte(args[i]);
                    i++;
                    keyColor.g = System.Convert.ToByte(args[i]);
                    i++;
                    keyColor.b = System.Convert.ToByte(args[i]);
                }
            }

            CustomFontRequest fontReq = new CustomFontRequest(font, charWidth, charHeight, nbCharH, nbCharV, inRow, keyColor);
            if (fullscreenWidth > 0)
                TCODSystem.ForceFullscrenResolution(fullscreenWidth, fullscreenHeight);

            rootConsole = new TCODConsoleRoot(80, 50, "tcodlib C# sample", fullscreen, fontReq);
            sampleConsole = rootConsole.GetNewConsole(SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);
            TCODKeyboard keyboard = new TCODKeyboard();

            setupStaticData();
            do
            {
                rootConsole.Clear();
                for (int i = 0; i < sampleList.Length; i++)
                {
                    if (i == curSample)
                    {
                        // set colors for currently selected sample
                        rootConsole.SetForegroundColor(TCODColor.TCOD_white);
                        rootConsole.SetBackgroundColor(TCODColor.TCOD_light_blue);
                    }
                    else
                    {
                        // set colors for other samples
                        rootConsole.SetForegroundColor(TCODColor.TCOD_grey);
                        rootConsole.SetBackgroundColor(TCODColor.TCOD_black);
                    }
                    rootConsole.PrintLine(sampleList[i].name, 2, 47 - (sampleList.Length - i) * 2, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET), TCODLineAlign.Left);
                }
                rootConsole.SetForegroundColor(TCODColor.TCOD_grey);
                rootConsole.SetBackgroundColor(TCODColor.TCOD_black);
                rootConsole.PrintLine("last frame : " + (TCODSystem.GetLastFrameLength() * 1000).ToString() + " ms ( " + TCODSystem.GetFPS() + "fps)", 79, 46, TCODLineAlign.Right);
                rootConsole.PrintLine("elapsed : " + TCODSystem.GetElapsedMilli() + "ms " + (TCODSystem.GetElapsedSeconds().ToString("0.00")) + "s", 79, 47, TCODLineAlign.Right);
                rootConsole.PrintLine(TCODSpecialChar.TCOD_CHAR_ARROW_N + TCODSpecialChar.TCOD_CHAR_ARROW_S + " : select a sample", 2, 47, TCODLineAlign.Left);
                rootConsole.PrintLine("ALT-ENTER : switch to " + (rootConsole.IsFullscreen() ? "windowed mode  " : "fullscreen mode"), 2, 48, TCODLineAlign.Left);

                sampleList[curSample].render(first, key);
                first = false;

                sampleConsole.Blit(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, rootConsole, SAMPLE_SCREEN_X, SAMPLE_SCREEN_Y, 255);

                rootConsole.Flush();
                key = keyboard.CheckForKeypress(TCOD_keypressed.TCOD_KEY_PRESSED);

                if (key.vk == TCOD_keycode.TCODK_DOWN)
                {
                    // down arrow : next sample
                    curSample = (curSample + 1) % sampleList.Length;
                    first = true;
                }
                else if (key.vk == TCOD_keycode.TCODK_UP)
                {
                    // up arrow : previous sample
                    curSample--;
                    if (curSample < 0)
                        curSample = sampleList.Length - 1;
                    first = true;
                }
                else if (key.vk == TCOD_keycode.TCODK_ENTER && key.lalt)
                {
                    // ALT-ENTER : switch fullscreen
                    rootConsole.SetFullscreen(!rootConsole.IsFullscreen());
                }
                else if (key.vk == TCOD_keycode.TCODK_F1)
                {
                    System.Console.Out.WriteLine("key.pressed" + " " + key.lalt + " " + key.lctrl + " " + key.ralt + " " + key.rctrl + " " + key.shift);
                }

            }
            while (!rootConsole.IsWindowClosed());
            return 0;
        }

        public static int Main(string[] args)
        {
            using (TCODDemo d = new TCODDemo())
            {
                return d.Run(args);
            }
        }
    };
}