using System;
using libtcodWrapper;

using Console = libtcodWrapper.Console;
using System.Collections.Generic;

// This is a direct port of sample_c from libtcod.
// This was done to make obvious the differences between the two APIs
// This is not suggested as a style to use for c# programs.
namespace TCODDemo
{
    class TCODDemo : IDisposable
    {
        Console sampleConsole;

        const int SAMPLE_SCREEN_WIDTH = 46;
        const int SAMPLE_SCREEN_HEIGHT = 20;

        const int SAMPLE_SCREEN_X = 20;
        const int SAMPLE_SCREEN_Y = 10;

        delegate void SampleRender(bool first, KeyPress key);

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
        { }

        private bool ArgsRemaining(string[] args, int pos, int numberRequested)
        {
            return (pos + numberRequested < args.Length);
        }

        Color[] render_cols;
        int[] render_dirr;
        int[] render_dirg;
        int[] render_dirb;
        TCODRandom random;
        void render_colors(bool first, KeyPress key)
        {
            int TOPLEFT = 0;
            int TOPRIGHT = 1;
            int BOTTOMLEFT = 2;
            int BOTTOMRIGHT = 3;

            Color textColor = new Color();

            /* ==== slighty modify the corner colors ==== */
            if (first)
            {
                TCODSystem.FPS = 0;
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
                        render_cols[c] = Color.FromRGB((byte)(render_cols[c].Red + (byte)(5 * render_dirr[c])),
                                render_cols[c].Green, render_cols[c].Blue);
                        if (render_cols[c].Red == 255)
                            render_dirr[c] = -1;
                        else if (render_cols[c].Red == 0)
                            render_dirr[c] = 1;
                        break;
                    case 1:
                        render_cols[c] = Color.FromRGB(render_cols[c].Red,
                                (byte)(render_cols[c].Green + (byte)(5 * render_dirg[c])), render_cols[c].Blue);
                        if (render_cols[c].Green == 255)
                            render_dirg[c] = -1;
                        else if (render_cols[c].Green == 0)
                            render_dirg[c] = 1;
                        break;
                    case 2:
                        render_cols[c] = Color.FromRGB(render_cols[c].Red,
                                render_cols[c].Green, (byte)(render_cols[c].Blue + (byte)(5 * render_dirb[c])));
                        if (render_cols[c].Blue == 255)
                            render_dirb[c] = -1;
                        else if (render_cols[c].Blue == 0)
                            render_dirb[c] = 1;
                        break;
                }
            }

            /* ==== scan the whole screen, interpolating corner colors ==== */
            for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
            {
                float xcoef = (float)(x) / (SAMPLE_SCREEN_WIDTH - 1);
                /* get the current column top and bottom colors */
                Color top = Color.Interpolate(render_cols[TOPLEFT], render_cols[TOPRIGHT], xcoef);
                Color bottom = Color.Interpolate(render_cols[BOTTOMLEFT], render_cols[BOTTOMRIGHT], xcoef);
                for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                {
                    float ycoef = (float)(y) / (SAMPLE_SCREEN_HEIGHT - 1);
                    /* get the current cell color */
                    Color curColor = Color.Interpolate(top, bottom, ycoef);
                    sampleConsole.SetCharBackground(x, y, curColor, Background.Set);
                }
            }

            /* ==== print the text ==== */
            /* get the background color at the text position */
            textColor = sampleConsole.GetCharBackground(SAMPLE_SCREEN_WIDTH / 2, 5);
            /* and invert it */
            textColor = Color.FromRGB((byte)(255 - textColor.Red),
                (byte)(255 - textColor.Green), (byte)(255 - textColor.Blue));
            sampleConsole.ForegroundColor = textColor;

            /* put random text (for performance tests) */
            for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
            {
                for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                {
                    Color col = sampleConsole.GetCharBackground(x, y);
                    col = Color.Interpolate(col, ColorPresets.Black, 0.5f);
                    int c = random.GetRandomInt(System.Convert.ToByte('a'), System.Convert.ToByte('z'));
                    sampleConsole.ForegroundColor = col;
                    sampleConsole.PutChar(x, y, (char)c, Background.None);
                }
            }

            /* the background behind the text is slightly darkened using the BKGND_MULTIPLY flag */
            sampleConsole.BackgroundColor = ColorPresets.Gray;
            sampleConsole.PrintLineRect("The Doryen library uses 24 bits colors, " +
                "for both background and foreground.",
                SAMPLE_SCREEN_WIDTH / 2, 5, SAMPLE_SCREEN_WIDTH - 2,
                SAMPLE_SCREEN_HEIGHT - 1, new Background(BackgroundFlag.Multiply),
                LineAlignment.Center);
        }




        Console off_secondary;
        Console off_screenshot;
        bool off_init = false; // draw the secondary screen only the first time
        int off_counter = 0;
        int off_x = 0, off_y = 0; // secondary screen position
        int off_xdir = 1, off_ydir = 1; // movement direction
        void render_offscreen(bool first, KeyPress key)
        {
            if (!off_init)
            {
                off_init = true;
                off_secondary.DrawFrame(0, 0, SAMPLE_SCREEN_WIDTH / 2, SAMPLE_SCREEN_HEIGHT / 2, false, "Offscreen console");
                off_secondary.PrintLineRect("You can render to an offscreen console and blit in on another one, simulating alpha transparency.", SAMPLE_SCREEN_WIDTH / 4, 2, SAMPLE_SCREEN_WIDTH / 2 - 2, SAMPLE_SCREEN_HEIGHT / 2, LineAlignment.Center);

            }
            if (first)
            {
                off_x = 0;
                off_y = 0;
                TCODSystem.FPS = 30; // fps limited to 30
                // get a "screenshot" of the current sample screen
                sampleConsole.Blit(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, off_screenshot, 0, 0);

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

            // restore the initial screen
            off_screenshot.Blit(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, sampleConsole, 0, 0);

            // blit the overlapping screen
            off_secondary.Blit(0, 0, SAMPLE_SCREEN_WIDTH / 2, SAMPLE_SCREEN_HEIGHT / 2, sampleConsole, off_x, off_y, 1.0f ,.75f);
        }

        static Console line_bk;
        static Background line_bkFlag = Background.Set;
        static bool line_init = false;
        void render_lines(bool first, KeyPress key)
        {
            sampleConsole.Clear();
            if (key.KeyCode == KeyCode.TCODK_ENTER || key.KeyCode == KeyCode.TCODK_KPENTER)
            {
                // switch to the next blending mode
                if (line_bkFlag.BackgroundFlag == BackgroundFlag.Alph)
                    line_bkFlag = Background.None;
                else
                    line_bkFlag++;
            }
            if (line_bkFlag.BackgroundFlag == BackgroundFlag.Alph)
            {
                // for the alpha mode, update alpha every frame
                double alpha = (1.0f + Math.Cos(TCODSystem.ElapsedSeconds * 2)) / 2.0f;
                line_bkFlag = new Background(BackgroundFlag.Alph, alpha);
            }
            else if (line_bkFlag.BackgroundFlag == BackgroundFlag.AddA)
            {
                // for the add alpha mode, update alpha every frame
                double alpha = (1.0f + Math.Cos(TCODSystem.ElapsedSeconds * 2)) / 2.0f;
                line_bkFlag = new Background(BackgroundFlag.AddA, alpha);
            }

            if (!line_init)
            {
                // initialize the colored background
                for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                {
                    for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                    {
                        Color col = Color.FromRGB((byte)(x * 255 / (SAMPLE_SCREEN_WIDTH - 1)),
                                        (byte)((x + y) * 255 / (SAMPLE_SCREEN_WIDTH - 1 + SAMPLE_SCREEN_HEIGHT - 1)),
                                        (byte)(y * 255 / (SAMPLE_SCREEN_HEIGHT - 1)));

                        line_bk.SetCharBackground(x, y, col, Background.Set);
                    }
                }
                line_init = true;
            }
            if (first)
            {
                TCODSystem.FPS = 30; // fps limited to 30
                sampleConsole.ForegroundColor = ColorPresets.White;
            }

            // blit the background
            line_bk.Blit(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, sampleConsole, 0, 0);



            // render the gradient
            int recty = (int)((SAMPLE_SCREEN_HEIGHT - 2) * ((1.0f + Math.Cos(TCODSystem.ElapsedSeconds)) / 2.0f));
            for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
            {
                Color col = Color.FromRGB((byte)(x * 255 / SAMPLE_SCREEN_WIDTH),
                                    (byte)(x * 255 / SAMPLE_SCREEN_WIDTH),
                                    (byte)(x * 255 / SAMPLE_SCREEN_WIDTH));
                sampleConsole.SetCharBackground(x, recty, col, line_bkFlag);
                sampleConsole.SetCharBackground(x, recty + 1, col, line_bkFlag);
                sampleConsole.SetCharBackground(x, recty + 2, col, line_bkFlag);
            }

            // calculate the segment ends
            float angle = TCODSystem.ElapsedSeconds * 2.0f;
            float cosAngle = (float)Math.Cos(angle);
            float sinAngle = (float)Math.Sin(angle);
            int xo = (int)(SAMPLE_SCREEN_WIDTH / 2 * (1 + cosAngle));
            int yo = (int)(SAMPLE_SCREEN_HEIGHT / 2 + sinAngle * SAMPLE_SCREEN_WIDTH / 2);
            int xd = (int)(SAMPLE_SCREEN_WIDTH / 2 * (1 - cosAngle));
            int yd = (int)(SAMPLE_SCREEN_HEIGHT / 2 - sinAngle * SAMPLE_SCREEN_WIDTH / 2);

            // render the line
            int xx = xo, yy = yo;
            TCODLineDrawing.InitLine(xx, yy, xd, yd);
            do
            {
                if (xx >= 0 && yy >= 0 && xx < SAMPLE_SCREEN_WIDTH && yy < SAMPLE_SCREEN_HEIGHT)
                {
                    sampleConsole.SetCharBackground(xx, yy, ColorPresets.Blue, line_bkFlag);
                }
            }
            while (!TCODLineDrawing.StepLine(ref xx, ref yy));

            // print the current flag
            sampleConsole.PrintLine(line_bkFlag.BackgroundFlag.ToString() + " (ENTER to change)", 2, 2, LineAlignment.Left);
        }

        enum noiseFunctions { Perlin, Simplex, Wavelet, PerlinFBM, SimplexFBM, WaveletFBM, PerlinTurbulence, SimplexTurbulence, WaveletTurbulence };
        string[] noise_funcName = 
        {
            "1 : perlin noise       ",
            "2 : simplex noise      ",
            "3 : wavelet noise      ",
            "4 : perlin fbm         ",
            "5 : perlin turbulence  ",
            "6 : simplex fbm        ",
            "7 : simplex turbulence ",
            "8 : wavelet fbm        ",
            "9 : wavelet turbulence ",

        };
        noiseFunctions noise_func = noiseFunctions.Perlin;
        TCODNoise noise;
        float noise_dx = 0.0f, noise_dy = 0.0f;
        float noise_octaves = 4.0f;
        float noise_hurst = TCODNoise.NoiseDefaultHurst;
        float noise_lacunarity = TCODNoise.NoiseDefaultLacunarity;
        float noise_zoom = 3.0f;
        void render_noise(bool first, KeyPress key)
        {
            if (first)
            {
                TCODSystem.FPS = 30; /* limited to 30 fps */
            }
            sampleConsole.Clear();

            /* texture animation */
            noise_dx += 0.01f;
            noise_dy += 0.01f;

            /* render the 2d noise function */
            for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                {
                    float[] f = new float[2];
                    float value = 0.0f;
                    byte c;
                    Color col = new Color();
                    f[0] = noise_zoom * x / SAMPLE_SCREEN_WIDTH + noise_dx;
                    f[1] = noise_zoom * y / SAMPLE_SCREEN_HEIGHT + noise_dy;

                    switch (noise_func)
                    {
                        case noiseFunctions.Perlin:
                            value = noise.GetPerlinNoise(f);
                            break;
                        case noiseFunctions.PerlinFBM:
                            value = noise.GetPerlinBrownianMotion(f, noise_octaves);
                            break;
                        case noiseFunctions.PerlinTurbulence:
                            value = noise.GetPerlinTurbulence(f, noise_octaves);
                            break;
                        case noiseFunctions.Simplex:
                            value = noise.GetSimplexNoise(f);
                            break;
                        case noiseFunctions.SimplexFBM:
                            value = noise.GetSimplexBrownianMotion(f, noise_octaves);
                            break;
                        case noiseFunctions.SimplexTurbulence:
                            value = noise.GetSimplexTurbulence(f, noise_octaves);
                            break;
                        case noiseFunctions.Wavelet:
                            value = noise.GetWaveletNoise(f);
                            break;
                        case noiseFunctions.WaveletFBM:
                            value = noise.GetWaveletBrownianMotion(f, noise_octaves);
                            break;
                        case noiseFunctions.WaveletTurbulence:
                            value = noise.GetWaveletTurbulence(f, noise_octaves);
                            break;
                    }

                    c = (byte)((value + 1.0f) / 2.0f * 255);
                    /* use a bluish color */
                    col = Color.FromRGB((byte)(c / 2), (byte)(c / 2), c);
                    sampleConsole.SetCharBackground(x, y, col, Background.Set);
                }
            }

            /* draw a transparent rectangle */
            sampleConsole.BackgroundColor = ColorPresets.Gray;
            sampleConsole.DrawRect(2, 2, (noise_func <= noiseFunctions.Wavelet ? 16 : 24), (noise_func <= noiseFunctions.Wavelet ? 4 : 7), false, new Background(BackgroundFlag.Multiply));

            /* draw the text */
            for (noiseFunctions curfunc = noiseFunctions.Perlin; curfunc <= noiseFunctions.WaveletTurbulence; curfunc++)
            {
                if (curfunc == noise_func)
                {
                    sampleConsole.ForegroundColor = ColorPresets.White;
                    sampleConsole.BackgroundColor = ColorPresets.Blue;
                    sampleConsole.PrintLine(noise_funcName[(int)curfunc], 2, 2 + (int)(curfunc), Background.Set, LineAlignment.Left);
                }
                else
                {
                    sampleConsole.ForegroundColor = ColorPresets.Gray;
                    sampleConsole.PrintLine(noise_funcName[(int)curfunc], 2, 2 + (int)(curfunc), Background.None, LineAlignment.Left);
                }
            }
            /* draw parameters */
            sampleConsole.ForegroundColor = ColorPresets.White;
            sampleConsole.PrintLine("Y/H : zome (" + noise_zoom.ToString("0.0") + ")", 2, 11, LineAlignment.Left);

            if (noise_func > noiseFunctions.Wavelet)
            {
                sampleConsole.PrintLine("E/D : hurst (" + noise_hurst.ToString("0.0") + ")", 2, 12, LineAlignment.Left);
                sampleConsole.PrintLine("R/F : lacunarity (" + noise_lacunarity.ToString("0.0") + ")", 2, 13, LineAlignment.Left);
                sampleConsole.PrintLine("T/G : octaves (" + noise_octaves.ToString("0.0") + ")", 2, 14, LineAlignment.Left);
            }

            /* handle keypress */
            if (key.KeyCode == KeyCode.TCODK_NONE)
                return;
            if (key.Character >= '1' && key.Character <= '9')
            {
                noise_func = (noiseFunctions)(key.Character - '1');
            }
            else if (key.Character == 'E' || key.Character == 'e')
            {
                /* increase hurst */
                noise_hurst += 0.1f;
                noise.Dispose();
                noise = new TCODNoise(2, noise_hurst, noise_lacunarity);
            }
            else if (key.Character == 'D' || key.Character == 'd')
            {
                /* decrease hurst */
                noise_hurst -= 0.1f;
                noise.Dispose();
                noise = new TCODNoise(2, noise_hurst, noise_lacunarity);
            }
            else if (key.Character == 'R' || key.Character == 'r')
            {
                /* increase lacunarity */
                noise_lacunarity += 0.5f;
                noise.Dispose();
                noise = new TCODNoise(2, noise_hurst, noise_lacunarity);
            }
            else if (key.Character == 'F' || key.Character == 'f')
            {
                /* decrease lacunarity */
                noise_lacunarity -= 0.5f;
                noise.Dispose();
                noise = new TCODNoise(2, noise_hurst, noise_lacunarity);
            }
            else if (key.Character == 'T' || key.Character == 't')
            {
                /* increase octaves */
                noise_octaves += 0.5f;
            }
            else if (key.Character == 'G' || key.Character == 'g')
            {
                /* decrease octaves */
                noise_octaves -= 0.5f;
            }
            else if (key.Character == 'Y' || key.Character == 'y')
            {
                /* increase zoom */
                noise_zoom += 0.2f;
            }
            else if (key.Character == 'H' || key.Character == 'h')
            {
                /* decrease zoom */
                noise_zoom -= 0.2f;
            }

        }

        const float TORCH_RADIUS = 10.0f;
        const float SQUARED_TORCH_RADIUS = (TORCH_RADIUS * TORCH_RADIUS);
        int px = 20, py = 10; // player position
        bool recomputeFov = true; // the player moved. must recompute fov
        bool torch = false; // torch fx on ?
        bool light_walls = true;
        TCODFov map;
        Color darkWall = Color.FromRGB(0, 0, 100);
        Color lightWall = Color.FromRGB(130, 110, 50);
        Color darkGround = Color.FromRGB(50, 50, 150);
        Color lightGround = Color.FromRGB(200, 180, 50);
        TCODNoise map_noise;
        float torchx = 0.0f; // torch light position in the perlin noise

        int algonum = 0;
        string[] algo_names = { "BASIC      ", "DIAMOND    ", "SHADOW     ", 
		                        "PERMISSIVE0","PERMISSIVE1","PERMISSIVE2","PERMISSIVE3","PERMISSIVE4",
		                        "PERMISSIVE5","PERMISSIVE6","PERMISSIVE7","PERMISSIVE8", "RESTRICTIVE"};

        #region Map
        string[] smap = {
        "##############################################",
        "#######################      #################",
        "#####################    #     ###############",
        "######################  ###        ###########",
        "##################      #####             ####",
        "################       ########    ###### ####",
        "###############      #################### ####",
        "################    ######                  ##",
        "########   #######  ######   #     #     #  ##",
        "########   ######      ###                  ##",
        "########                                    ##",
        "####       ######      ###   #     #     #  ##",
        "#### ###   ########## ####                  ##",
        "#### ###   ##########   ###########=##########",
        "#### ##################   #####          #####",
        "#### ###             #### #####          #####",
        "####           #     ####                #####",
        "########       #     #### #####          #####",
        "########       #####      ####################",
        "##############################################"
        };
        #endregion

        void render_fov(bool first, KeyPress key)
        {
            if (map == null)
            {
                // initialize the map for the fov toolkit
                map = new TCODFov(SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);
                for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                {
                    for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                    {
                        if (smap[y][x] == ' ')
                            map.SetCell(x, y, true, true);// ground
                        else if (smap[y][x] == '=')
                            map.SetCell(x, y, true, false); // window
                    }
                }
                // 1d noise used for the torch flickering
                map_noise = new TCODNoise(1);
            }

            if (first)
            {
                TCODSystem.FPS = 30; // fps limited to 30
                // we draw the foreground only the first time.
                // during the player movement, only the @ is redrawn.
                // the rest impacts only the background color
                // draw the help text & player @
                sampleConsole.Clear();
                sampleConsole.ForegroundColor = (ColorPresets.White);
                sampleConsole.PrintLine("IJKL : move around\nT : torch fx " + (torch ? "off" : "on ") + " : light walls " + (light_walls ? "off" : "on ") + "\n+-: algo " + algo_names[algonum], 1, 1, LineAlignment.Left);
                sampleConsole.ForegroundColor = (ColorPresets.Black);
                sampleConsole.PutChar(px, py, '@');
                // draw windows
                for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                {
                    for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                    {
                        if (smap[y][x] == '=')
                        {
                            sampleConsole.PutChar(x, y, '=');
                        }
                    }
                }
            }

            if (recomputeFov)
            {
                // calculate the field of view from the player position
                recomputeFov = false;
                map.CalculateFOV(px, py, torch ? (int)TORCH_RADIUS : 0, light_walls, (FovAlgorithm)algonum);
            }
            // torch position & intensity variation
            float dx = 0.0f, dy = 0.0f, di = 0.0f;
            if (torch)
            {
                // slightly change the perlin noise parameter
                torchx += 0.2f;
                // randomize the light position between -1.5 and 1.5
                float[] tdx = { torchx + 20.0f };
                dx = map_noise.GetPerlinNoise(tdx) * 1.5f;
                tdx[0] += 30.0f;
                dy = map_noise.GetPerlinNoise(tdx) * 1.5f;
                // randomize the light intensity between -0.2 and 0.2
                float[] torchxArray = { torchx };
                di = 0.2f * map_noise.GetPerlinNoise(torchxArray);
            }

            // draw the dungeon
            for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                {
                    bool visible = map.CheckTileFOV(x, y);
                    bool wall = (smap[y][x] == '#');
                    if (!visible)
                    {
                        sampleConsole.SetCharBackground(x, y, (wall ? darkWall : darkGround), Background.Set);
                    }
                    else
                    {
                        if (!torch)
                        {
                            sampleConsole.SetCharBackground(x, y, wall ? lightWall : lightGround, Background.Set);
                        }
                        else
                        {
                            // torch flickering fx
                            Color baseColor = wall ? darkWall : darkGround;
                            Color light = wall ? lightWall : lightGround;
                            // cell distance to torch (squared)
                            float r = (float)((x - px + dx) * (x - px + dx) + (y - py + dy) * (y - py + dy));
                            if (r < SQUARED_TORCH_RADIUS)
                            {
                                // l = 1.0 at player position, 0.0 at a radius of 10 cells
                                float l = (SQUARED_TORCH_RADIUS - r) / SQUARED_TORCH_RADIUS + di;
                                // clamp between 0 and 1
                                if (l < 0.0f)
                                    l = 0.0f;
                                else if (l > 1.0f)
                                    l = 1.0f;
                                // interpolate the color
                                baseColor = Color.FromRGB((byte)(baseColor.Red + (light.Red - baseColor.Red) * l),
                                                    (byte)(baseColor.Green + (light.Green - baseColor.Green) * l),
                                                    (byte)(baseColor.Blue + (light.Blue - baseColor.Blue) * l));
                            }
                            sampleConsole.SetCharBackground(x, y, baseColor, Background.Set);
                        }
                    }
                }
            }

            if (key.Character == 'I' || key.Character == 'i')
            {
                // player move north
                if (smap[py - 1][px] == ' ')
                {
                    sampleConsole.PutChar(px, py, ' ', Background.None);
                    py--;
                    sampleConsole.PutChar(px, py, '@', Background.None);
                    recomputeFov = true;
                }
            }
            else if (key.Character == 'K' || key.Character == 'k')
            {
                // player move south
                if (smap[py + 1][px] == ' ')
                {
                    sampleConsole.PutChar(px, py, ' ', Background.None);
                    py++;
                    sampleConsole.PutChar(px, py, '@', Background.None);
                    recomputeFov = true;
                }
            }
            else if (key.Character == 'J' || key.Character == 'j')
            {
                // player move west
                if (smap[py][px - 1] == ' ')
                {
                    sampleConsole.PutChar(px, py, ' ', Background.None);
                    px--;
                    sampleConsole.PutChar(px, py, '@', Background.None);
                    recomputeFov = true;
                }
            }
            else if (key.Character == 'L' || key.Character == 'l')
            {
                // player move east
                if (smap[py][px + 1] == ' ')
                {
                    sampleConsole.PutChar(px, py, ' ', Background.None);
                    px++;
                    sampleConsole.PutChar(px, py, '@', Background.None);
                    recomputeFov = true;
                }
            }
            else if (key.Character == 'T' || key.Character == 't')
            {
                // enable/disable the torch fx
                torch = !torch;
                sampleConsole.ForegroundColor = (ColorPresets.White);
                sampleConsole.PrintLine("IJKL : move around\nT : torch fx " + (torch ? "off" : "on ") + " : light walls " + (light_walls ? "off" : "on ") + "\n+-: algo " + algo_names[algonum], 1, 1, LineAlignment.Left);
                sampleConsole.ForegroundColor = (ColorPresets.Black);
            }
            else if (key.Character == 'W' || key.Character == 'W')
            {
                light_walls = !light_walls;
                sampleConsole.ForegroundColor = (ColorPresets.White);
                sampleConsole.PrintLine("IJKL : move around\nT : torch fx " + (torch ? "off" : "on ") + " : light walls " + (light_walls ? "off" : "on ") + "\n+-: algo " + algo_names[algonum], 1, 1, LineAlignment.Left);
                sampleConsole.ForegroundColor = (ColorPresets.Black);
                recomputeFov = true;
            }
            else if (key.Character == '+' || key.Character == '-')
            {
                algonum += key.Character == '+' ? 1 : -1;

                if (algonum >= (int)FovAlgorithm.NB_Fov_Algorithms)
                    algonum = (int)FovAlgorithm.Restrictive;
                else if (algonum < 0)
                    algonum = 0;

                sampleConsole.ForegroundColor = (ColorPresets.White);
                sampleConsole.PrintLine("IJKL : move around\nT : torch fx " + (torch ? "off" : "on ") + " : light walls " + (light_walls ? "off" : "on ") + "\n+-: algo " + algo_names[algonum], 1, 1, LineAlignment.Left);
                sampleConsole.ForegroundColor = (ColorPresets.Black);
                recomputeFov = true;
            }
        }

        Image img;
        Image circle;
        Color blue = Color.FromRGB(0, 0, 255);
        Color green = Color.FromRGB(0, 255, 0);
        uint lastSwitch = 0;
        bool swap = false;
        void render_image(bool first, KeyPress key)
        {
            sampleConsole.Clear();

            if (img == null)
            {
                img = new Image("skull.png");
                circle = new Image("circle.png");
            }

            if (first)
                TCODSystem.FPS = 30;  /* limited to 30 fps */

            sampleConsole.BackgroundColor = ColorPresets.Black;
            sampleConsole.Clear();

            float x = SAMPLE_SCREEN_WIDTH / 2 + (float)Math.Cos(TCODSystem.ElapsedSeconds) * 10.0f;
            float y = (float)(SAMPLE_SCREEN_HEIGHT / 2);
            float scalex = 0.2f + 1.8f * (1.0f + (float)Math.Cos(TCODSystem.ElapsedSeconds / 2)) / 2.0f;
            float scaley = scalex;
            float angle = TCODSystem.ElapsedSeconds;
            uint elapsed = TCODSystem.ElapsedMilliseconds / 2000;

            if (elapsed > lastSwitch)
            {
                lastSwitch = elapsed;
                swap = !swap;
            }

            if (swap)
            {
                /* split the color channels of circle.png */
                /* the red channel */
                sampleConsole.BackgroundColor = (ColorPresets.Red);
                sampleConsole.DrawRect(0, 3, 15, 15, false, Background.Set);
                circle.BlitRect(sampleConsole, 0, 3, -1, -1, new Background(BackgroundFlag.Multiply));
                /* the green channel */
                sampleConsole.BackgroundColor = (green);
                sampleConsole.DrawRect(15, 3, 15, 15, false, Background.Set);
                circle.BlitRect(sampleConsole, 15, 3, -1, -1, new Background(BackgroundFlag.Multiply));
                /* the blue channel */
                sampleConsole.BackgroundColor = (blue);
                sampleConsole.DrawRect(30, 3, 15, 15, false, Background.Set);
                circle.BlitRect(sampleConsole, 30, 3, -1, -1, new Background(BackgroundFlag.Multiply));
            }
            else
            {
                /* render circle.png with normal blitting */
                circle.BlitRect(sampleConsole, 0, 3, -1, -1, Background.Set);
                circle.BlitRect(sampleConsole, 15, 3, -1, -1, Background.Set);
                circle.BlitRect(sampleConsole, 30, 3, -1, -1, Background.Set);
            }
            img.Blit(sampleConsole, x, y, new Background(BackgroundFlag.AddA, .6), scalex, scaley, angle);
        }

        bool mouse_lbut = false, mouse_rbut = false, mouse_mbut = false;
        void render_mouse(bool first, KeyPress key)
        {
            if (first)
            {
                sampleConsole.BackgroundColor = (ColorPresets.Gray);
                sampleConsole.ForegroundColor = (ColorPresets.Yellow);
                Mouse.MoveMouse(320, 200);
                Mouse.ShowCursor(true);
            }

            sampleConsole.Clear();
            Mouse mouse = Mouse.GetStatus();

            if (mouse.LeftButtonPressed)
                mouse_lbut = !mouse_lbut;
            if (mouse.RightButtonPressed)
                mouse_rbut = !mouse_rbut;
            if (mouse.MiddleButtonPressed)
                mouse_mbut = !mouse_mbut;

            string s1 = "Mouse position : " + mouse.PixelX.ToString("000") + " x " + mouse.PixelY.ToString("000") + "\n";
            string s2 = "Mouse cell     : " + mouse.CellX.ToString("000") + " x " + mouse.CellY.ToString("000") + "\n";
            string s3 = "Mouse movement : " + mouse.PixelVelocityX.ToString("000") + " x " + mouse.PixelVelocityY.ToString("000") + "\n";
            string s4 = "Left button    : " + (mouse.LeftButton ? " ON" : "OFF") + " (toggle " + (mouse_lbut ? " ON" : "OFF") + ")\n";
            string s5 = "Right button   : " + (mouse.RightButton ? " ON" : "OFF") + " (toggle " + (mouse_rbut ? " ON" : "OFF") + ")\n";
            string s6 = "Middle button  : " + (mouse.MiddleButton ? " ON" : "OFF") + " (toggle " + (mouse_mbut ? " ON" : "OFF") + ")\n";

            string s = s1 + s2 + s3 + s4 + s5 + s6;

            sampleConsole.PrintLine(s, 1, 1, LineAlignment.Left);

            sampleConsole.PrintLine("1 : Hide cursor\n2 : Show cursor", 1, 10, LineAlignment.Left);
            if (key.Character == '1')
                Mouse.ShowCursor(false);
            else if (key.Character == '2')
                Mouse.ShowCursor(true);
        }

        // int px = 20, py = 10; // player position
        int dx = 24, dy = 1; // destination
        TCODAStrPathFinding AStrPath = null;
        TCODDijkstraPathFinding DijkstraPath = null;
        bool recalculatePath = false;
        float busy = 0.0f;
        char oldChar = ' ';
        bool usingAstar = true;
        float dijkstraDist = 0;

        void render_path(bool first, KeyPress key)
        {
            if (map == null)
            {
                // initialize the map
                map = new TCODFov(SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);
                for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                {
                    for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                    {
                        if (smap[y][x] == ' ')
                            map.SetCell(x, y, true, true);// ground
                        else if (smap[y][x] == '=')
                            map.SetCell(x, y, true, false); // window
                    }
                }
            }

            if (first)
            {
                TCODSystem.FPS = 30; // fps limited to 30
                // we draw the foreground only the first time.
                // during the player movement, only the @ is redrawn.
                // the rest impacts only the background color
                // draw the help text & player @
                sampleConsole.Clear();
                sampleConsole.ForegroundColor = (ColorPresets.White);
                sampleConsole.PrintLine("IJKL / mouse :\nmove destination\nTAB : A*/dijkstra", 1, 1, LineAlignment.Left);
                sampleConsole.PrintLine("Using : A*", 1, 4, LineAlignment.Left);
                sampleConsole.ForegroundColor = (ColorPresets.Black);
                sampleConsole.PutChar(px, py, '@');
                // draw windows
                for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                {
                    for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                    {
                        if (smap[y][x] == '=')
                        {
                            sampleConsole.PutChar(x, y, '=');
                        }
                    }
                }
                recalculatePath = true;
            }
            
            if (recalculatePath)
            {
                if (usingAstar)
                {
                    if (AStrPath == null)
                        AStrPath = new TCODAStrPathFinding(map, 1.41);

                    AStrPath.ComputePath(px, py, dx, dy);
                }
                else
                {
                    if (DijkstraPath == null)
                        DijkstraPath = new TCODDijkstraPathFinding(map, 1.41);

                    dijkstraDist = 0.0f;
                    /* compute the distance grid */
                    DijkstraPath.Compute(px, py);
                    /* get the maximum distance (needed for ground shading only) */
                    for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                    {
                        for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                        {
                            float d = DijkstraPath.GetDistance(x, y);
                            if (d > dijkstraDist) 
                                dijkstraDist = d;
                        }
                    }
                    // compute the path
                    DijkstraPath.SetPath(dx, dy);
                }
                recalculatePath = false;
                busy = .2f;
            }

            // draw the dungeon
            for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                {
                    bool wall = smap[y][x] == '#';
                    sampleConsole.SetCharBackground(x, y, (wall ? darkWall : darkGround), Background.Set);
                }
            }

            // draw the path
            if (usingAstar)
            {
                for (int i = 0; i < AStrPath.GetPathSize(); i++)
                {
                    int x, y;
                    AStrPath.GetPointOnPath(i, out x, out y);
                    sampleConsole.SetCharBackground(x, y, lightGround, Background.Set);
                }
            }
            else
            {
                for (int y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
                {
                    for (int x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                    {
                        bool wall = smap[y][x] == '#';
                        if (!wall)
                        {
                            float d = DijkstraPath.GetDistance(x, y);
                            sampleConsole.SetCharBackground(x, y, Color.Interpolate(lightGround, darkGround, 0.9 * d / dijkstraDist), Background.Set);
                        }
                    }
                }
                for (int i = 0; i < DijkstraPath.PathLength(); i++)
                {
                    int x, y;
                    DijkstraPath.GetPointOnPath(i, out x, out y);
                    sampleConsole.SetCharBackground(x, y, lightGround, Background.Set);
                }
            }

            // move the creature
            busy -= TCODSystem.LastFrameLength;
            if (busy <= 0.0f)
            {
                busy += 0.2f;
                if (usingAstar)
                {
                    if (!AStrPath.IsPathEmpty())
                    {
                        sampleConsole.PutChar(px, py, ' ', Background.None);
                        AStrPath.WalkPath(ref px, ref py, true);
                        sampleConsole.PutChar(px, py, '@', Background.None);
                    }
                }
                else
                {
                    if (!DijkstraPath.IsPathEmpty())
                    {
                        sampleConsole.PutChar(px, py, ' ', Background.None);
                        DijkstraPath.WalkPath(ref px, ref py);
                        sampleConsole.PutChar(px, py, '@', Background.None);
                        recalculatePath = true;
                    }
                }
            }

            if ((key.Character == 'I' || key.Character == 'i') && dy > 0)
            {
                // destination move north
                sampleConsole.PutChar(dx, dy, oldChar, Background.None);
                dy--;
                oldChar = sampleConsole.GetChar(dx, dy);
                sampleConsole.PutChar(dx, dy, '+', Background.None);
                if (smap[dy][dx] == ' ')
                {
                    recalculatePath = true;
                }
            }
            else if ((key.Character == 'K' || key.Character == 'k') && dy < SAMPLE_SCREEN_HEIGHT - 1)
            {
                // destination move south
                sampleConsole.PutChar(dx, dy, oldChar, Background.None);
                dy++;
                oldChar = sampleConsole.GetChar(dx, dy);
                sampleConsole.PutChar(dx, dy, '+', Background.None);
                if (smap[dy][dx] == ' ')
                {
                    recalculatePath = true;
                }
            }
            else if ((key.Character == 'J' || key.Character == 'j') && dx > 0)
            {
                // destination move west
                sampleConsole.PutChar(dx, dy, oldChar, Background.None);
                dx--;
                oldChar = sampleConsole.GetChar(dx, dy);
                sampleConsole.PutChar(dx, dy, '+', Background.None);
                if (smap[dy][dx] == ' ')
                {
                    recalculatePath = true;
                }
            }
            else if ((key.Character == 'L' || key.Character == 'l') && dx < SAMPLE_SCREEN_WIDTH - 1)
            {
                // destination move east
                sampleConsole.PutChar(dx, dy, oldChar, Background.None);
                dx++;
                oldChar = sampleConsole.GetChar(dx, dy);
                sampleConsole.PutChar(dx, dy, '+', Background.None);
                if (smap[dy][dx] == ' ')
                {
                    recalculatePath = true;
                }
            }
            else if (key.KeyCode == KeyCode.TCODK_TAB)
            {
                usingAstar = !usingAstar;
                sampleConsole.ForegroundColor = (ColorPresets.White);
                if (usingAstar)
                    sampleConsole.PrintLine("Using : A*      ", 1, 4, LineAlignment.Left);
                else
                    sampleConsole.PrintLine("Using : Dijkstra", 1, 4, LineAlignment.Left);
                sampleConsole.ForegroundColor = (ColorPresets.Black);
                recalculatePath = true;
            }

            Mouse mouse = Mouse.GetStatus();
            int mx = mouse.CellX - SAMPLE_SCREEN_X;
            int my = mouse.CellY - SAMPLE_SCREEN_Y;

            if (mx >= 0 && mx < SAMPLE_SCREEN_WIDTH && my >= 0 && my < SAMPLE_SCREEN_HEIGHT && (dx != mx || dy != my))
            {
                sampleConsole.PutChar(dx, dy, oldChar, Background.None);
                dx = mx; dy = my;
                oldChar = sampleConsole.GetChar(dx, dy);
                sampleConsole.PutChar(dx, dy, '+', Background.None);
                if (smap[dy][dx] == ' ')
                {
                    recalculatePath = true;
                }
            }
        }


        // ***************************
        // bsp sample
        // ***************************
        #region "BSP Helpers"
        int bspDepth = 8;
        int minRoomSize = 4;
        bool randomRoom = false; // a room fills a random part of the node or the maximum available space ?
        bool roomWalls = true; // if true, there is always a wall on north & west side of a room

        // draw a vertical line
        void vline(char[,] map, int x, int y1, int y2)
        {
            int y = y1;
            int dy = (y1 > y2 ? -1 : 1);
            map[x,y] = ' ';
            if (y1 == y2) return;
            do
            {
                y += dy;
                map[x,y] = ' ';
            } while (y != y2);
        }

        // draw a vertical line up until we reach an empty space
        void vline_up(char[,] map, int x, int y)
        {
            while (y >= 0 && map[x,y] != ' ')
            {
                map[x,y] = ' ';
                y--;
            }
        }

        // draw a vertical line down until we reach an empty space
        void vline_down(char[,] map, int x, int y)
        {
            while (y < SAMPLE_SCREEN_HEIGHT && map[x,y] != ' ')
            {
                map[x,y] = ' ';
                y++;
            }
        }

        // draw a horizontal line
        void hline(char[,] map, int x1, int y, int x2)
        {
            int x = x1;
            int dx = (x1 > x2 ? -1 : 1);
            map[x,y] = ' ';
            if (x1 == x2) return;
            do
            {
                x += dx;
                map[x,y] = ' ';
            } while (x != x2);
        }

        // draw a horizontal line left until we reach an empty space
        void hline_left(char[,] map, int x, int y)
        {
            while (x >= 0 && map[x,y] != ' ')
            {
                map[x,y] = ' ';
                x--;
            }
        }

        // draw a horizontal line right until we reach an empty space
        void hline_right(char[,] map, int x, int y)
        {
            while (x < SAMPLE_SCREEN_WIDTH && map[x,y] != ' ')
            {
                map[x,y] = ' ';
                x++;
            }
        }
        
        // the class building the dungeon from the bsp nodes
        bool traverse_node(TCODBSP node)
        {           
            TCODRandom rnd = new TCODRandom();

            if (node.IsLeaf())
            {
                // calculate the room size
                int minx = node.x + 1;
                int maxx = node.x + node.w - 1;
                int miny = node.y + 1;
                int maxy = node.y + node.h - 1;
                int x, y;
                if (!roomWalls)
                {
                    if (minx > 1) minx--;
                    if (miny > 1) miny--;
                }
                if (maxx == SAMPLE_SCREEN_WIDTH - 1) maxx--;
                if (maxy == SAMPLE_SCREEN_HEIGHT - 1) maxy--;
                if (randomRoom)
                {
                    minx = rnd.GetRandomInt(minx, maxx - minRoomSize + 1);
                    miny = rnd.GetRandomInt(miny, maxy - minRoomSize + 1);
                    maxx = rnd.GetRandomInt(minx + minRoomSize - 1, maxx);
                    maxy = rnd.GetRandomInt(miny + minRoomSize - 1, maxy);
                }
                // resize the node to fit the room
                //	printf("node %dx%d %dx%d => room %dx%d %dx%d\n",node->x,node->y,node->w,node->h,minx,miny,maxx-minx+1,maxy-miny+1);
                node.x = minx;
                node.y = miny;
                node.w = maxx - minx + 1;
                node.h = maxy - miny + 1;
                // dig the room
                for (x = minx; x <= maxx; x++)
                {
                    for (y = miny; y <= maxy; y++)
                    {
                        bsp_map[x,y] = ' ';
                    }
                }
            }
            else
            {
                //	printf("lvl %d %dx%d %dx%d\n",node->level, node->x,node->y,node->w,node->h);
                // resize the node to fit its sons
                TCODBSP left = node.GetLeft();
                TCODBSP right = node.GetRight();

                node.x = System.Math.Min(left.x, right.x);
                node.y = System.Math.Min(left.y, right.y);
                node.w = System.Math.Max(left.x + left.w, right.x + right.w) - node.x;
                node.h = System.Math.Max(left.y + left.h, right.y + right.h) - node.y;
                // create a corridor between the two lower nodes
                if (node.horizontal)
                {
                    // vertical corridor
                    if (left.x + left.w - 1 < right.x || right.x + right.w - 1 < left.x)
                    {
                        // no overlapping zone. we need a Z shaped corridor
                        int x1 = rnd.GetRandomInt(left.x, left.x + left.w - 1);
                        int x2 = rnd.GetRandomInt(right.x, right.x + right.w - 1);
                        int y = rnd.GetRandomInt(left.y + left.h, right.y);
                        vline_up(bsp_map, x1, y - 1);
                        hline(bsp_map, x1, y, x2);
                        vline_down(bsp_map, x2, y + 1);
                    }
                    else
                    {
                        // straight vertical corridor
                        int minx = System.Math.Max(left.x, right.x);
                        int maxx = System.Math.Min(left.x + left.w - 1, right.x + right.w - 1);
                        int x = rnd.GetRandomInt(minx, maxx);
                        vline_down(bsp_map, x, right.y);
                        vline_up(bsp_map, x, right.y - 1);
                    }
                }
                else
                {
                    // horizontal corridor
                    if (left.y + left.h - 1 < right.y || right.y + right.h - 1 < left.y)
                    {
                        // no overlapping zone. we need a Z shaped corridor
                        int y1 = rnd.GetRandomInt(left.y, left.y + left.h - 1);
                        int y2 = rnd.GetRandomInt(right.y, right.y + right.h - 1);
                        int x = rnd.GetRandomInt(left.x + left.w, right.x);
                        hline_left(bsp_map, x - 1, y1);
                        vline(bsp_map, x, y1, y2);
                        hline_right(bsp_map, x + 1, y2);
                    }
                    else
                    {
                        // straight horizontal corridor
                        int miny = System.Math.Max(left.y, right.y);
                        int maxy = System.Math.Min(left.y + left.h - 1, right.y + right.h - 1);
                        int y = rnd.GetRandomInt(miny, maxy);
                        hline_left(bsp_map, right.x - 1, y);
                        hline_right(bsp_map, right.x, y);
                    }
                }
            }
            return true;
        }
        #endregion

        char[,] bsp_map = new char[SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT];

        TCODBSP bsp = null;
        bool generate = true;
        bool refresh = false;

        void render_bsp(bool first, KeyPress key)
        {
            int x, y;

            if (generate || refresh)
            {
                // dungeon generation
                if (bsp == null)
                {
                    // create the bsp
                    bsp = new TCODBSP(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);
                }
                else
                {
                    // restore the nodes size
                    bsp.Resize(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);
                }

                
                for (int x1 = 0; x1 < SAMPLE_SCREEN_WIDTH; x1++)
                    for (int y1 = 0; y1 < SAMPLE_SCREEN_HEIGHT; y1++)
                        bsp_map[x1, y1] = '#';
                
                  
                if (generate)
                {
                    // build a new random bsp tree
                    bsp.RemoveSons();
                    bsp.SplitRecursive(null, bspDepth, minRoomSize + (roomWalls ? 1 : 0), minRoomSize + (roomWalls ? 1 : 0), 1.5f, 1.5f);
                }
                // create the dungeon from the bsp
                bsp.TraverseInvertedOrder(new TCODBSPTraversalDelegate(traverse_node));
                generate = false;
                refresh = false;
            }

            sampleConsole.Clear();
            sampleConsole.ForegroundColor = (ColorPresets.White);
            sampleConsole.PrintLine("ENTER : rebuild bsp\nSPACE : rebuild dungeon\n+-: bsp depth " + bspDepth + "\n*/: room size " + minRoomSize + "\n1 : random room size " + (randomRoom ? "ON" : "OFF"), 1, 1, Background.None, LineAlignment.Left);

            if (randomRoom)
                sampleConsole.PrintLine("2 : room walls " + (roomWalls ? "ON" : "OFF"), 1, 6, Background.None, LineAlignment.Left);

            // render the level
            for (y = 0; y < SAMPLE_SCREEN_HEIGHT; y++)
            {
                for (x = 0; x < SAMPLE_SCREEN_WIDTH; x++)
                {
                    bool wall = (bsp_map[x,y] == '#');
                    sampleConsole.SetCharBackground(x, y, (wall ? darkWall : darkGround), Background.Set);
                }
            }

            if (key.KeyCode == KeyCode.TCODK_ENTER || key.KeyCode == KeyCode.TCODK_KPENTER)
            {
                generate = true;
            }
            else if (key.Character == ' ')
            {
                refresh = true;
            }
            else if (key.Character == '+')
            {
                bspDepth++;
                generate = true;
            }
            else if (key.Character == '-' && bspDepth > 1)
            {
                bspDepth--;
                generate = true;
            }
            else if (key.Character == '*')
            {
                minRoomSize++;
                generate = true;
            }
            else if (key.Character == '/' && minRoomSize > 2)
            {
                minRoomSize--;
                generate = true;
            }
            else if (key.Character == '1' || key.KeyCode == KeyCode.TCODK_1 || key.KeyCode == KeyCode.TCODK_KP1)
            {
                randomRoom = !randomRoom;
                if (!randomRoom) roomWalls = true;
                refresh = true;
            }
            else if (key.Character == '2' || key.KeyCode == KeyCode.TCODK_2 || key.KeyCode == KeyCode.TCODK_KP2)
            {
                roomWalls = !roomWalls;
                refresh = true;
            }
        }

        static int nbSets;
        static int curSet = 0;
        static float delay = 0.0f;
        static List<string> sets = new List<string>();
        static List<string> names = new List<string>();
        void render_name(bool first, KeyPress key)
        {
            if (names.Count == 0)
            {
                foreach (string filename in System.IO.Directory.GetFiles("Names"))
                {
                    TCODNameGenerator.LoadSyllableFile(filename);
                }
                sets = TCODNameGenerator.GetSet();
                nbSets = sets.Count;
            }
            if (first)
                TCODSystem.FPS = 30;

            while (names.Count >= 15)
            {
                string nameToRemove = names[0];
                names.RemoveAt(0);
            }
            sampleConsole.Clear();
            sampleConsole.ForegroundColor = TCODColorPresets.White;
            sampleConsole.PrintLine(string.Format("{0}\n\n+ : next generator\n- : prev generator", sets[curSet]), 1, 1, Background.None, LineAlignment.Left);

            for (int i = 0; i < names.Count; ++i)
            {
                string name = names[i];
                if (name.Length < SAMPLE_SCREEN_WIDTH)
                    sampleConsole.PrintLine(name, SAMPLE_SCREEN_WIDTH - 2, 2 + i, Background.None, LineAlignment.Right);
            }

            delay += TCODSystem.LastFrameLength;
            if (delay >= .5f)
            {
                delay -= .5f;
                names.Add(TCODNameGenerator.Generate(sets[curSet]));
            }
            if (key.Character == '+')
            {
                curSet++;
                if (curSet == nbSets)
                    curSet = 0;
                names.Add("======");
            }
            if (key.Character == '-')
            {
                curSet--;
                if (curSet < 0)
                    curSet = nbSets-1;
                names.Add("======");
            }
        }

        private sample[] sampleList = new sample[10];

        private void fillSampleList()
        {
            sampleList[0] = new sample("  True colors        ", render_colors);
            sampleList[1] = new sample("  Offscreen console  ", render_offscreen);
            sampleList[2] = new sample("  Line drawing       ", render_lines);
            sampleList[3] = new sample("  Noise              ", render_noise);
            sampleList[4] = new sample("  Field of view      ", render_fov);

            sampleList[5] = new sample("  Path finding       ", render_path);
            sampleList[6] = new sample("  Bsp toolkit        ", render_bsp);

            sampleList[7] = new sample("  Image toolkit      ", render_image);
            sampleList[8] = new sample("  Mouse support      ", render_mouse);
            sampleList[9] = new sample("  Name generator     ", render_name);
        }


        private void setupStaticData()
        {
            random = new TCODRandom();

            render_cols = new Color[4];
            render_cols[0] = Color.FromRGB(50, 40, 150);
            render_cols[1] = Color.FromRGB(240, 85, 5);
            render_cols[2] = Color.FromRGB(50, 35, 240);
            render_cols[3] = Color.FromRGB(10, 200, 130);

            render_dirr = new int[] { 1, -1, 1, 1 };
            render_dirg = new int[] { 1, -1, -1, 1 };
            render_dirb = new int[] { 1, 1, 1, -1 };

            off_secondary = RootConsole.GetNewConsole(SAMPLE_SCREEN_WIDTH / 2, SAMPLE_SCREEN_HEIGHT / 2);
            off_screenshot = RootConsole.GetNewConsole(SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);
            line_bk = RootConsole.GetNewConsole(SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);
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
            if (map != null)
                map.Dispose();
        }

        RootConsole rootConsole;
        public int Run(string[] args)
        {
            fillSampleList();

            int curSample = 0; // index of the current sample
            bool first = true; // first time we render a sample
            KeyPress key = new KeyPress();
            string font = "celtic_garamond_10x10_gs_tc.png";
            int numberCharsHorz = 32;
            int numberCharsVert = 8;
            int fullscreenWidth = 0;
            int fullscreenHeight = 0;
            bool fullscreen = false;
            bool credits = false;
            CustomFontRequestFontTypes flags = CustomFontRequestFontTypes.Grayscale | CustomFontRequestFontTypes.LayoutTCOD;
            CustomFontRequestFontTypes newFlags = 0;

            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] == "-font" && ArgsRemaining(args, i, 1))
                {
                    i++;
                    font = args[i];
                }
                else if (args[i] == "-font-char-numberRows" && ArgsRemaining(args, i, 2))
                {
                    i++;
                    numberCharsHorz = System.Convert.ToInt32(args[i]);
                    i++;
                    numberCharsVert = System.Convert.ToInt32(args[i]);
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
                    flags = 0;
                    newFlags |= CustomFontRequestFontTypes.LayoutAsciiInRow;
                }
                else if (args[i] == "-font-greyscale")
                {
                    flags = 0;
                    newFlags |= CustomFontRequestFontTypes.Grayscale;
                }
                else if (args[i] == "-font-tcod")
                {
                    flags = 0;
                    newFlags |= CustomFontRequestFontTypes.LayoutTCOD;
                }
                else if (args[i] == "-help")
                {
                    System.Console.Out.WriteLine("options : \n");
                    System.Console.Out.WriteLine("-font <filename> : use a custom font\n");
                    System.Console.Out.WriteLine("-font-char-size <char_width> <char_height> : size of the custom font's characters\n");
                    System.Console.Out.WriteLine("-font-in-row : the font layout is in row instead of columns\n");
                    System.Console.Out.WriteLine("-font-tcod : the font uses TCOD layout instead of ASCII\n");
                    System.Console.Out.WriteLine("-font-greyscale : antialiased font using greyscale bitmap\n");
                    System.Console.Out.WriteLine("-fullscreen : start in fullscreen\n");
                    System.Console.Out.WriteLine("-fullscreen-resolution <screen_width> <screen_height> : force fullscreen resolution\n");

                }
            }
            if (flags == 0)
                flags = newFlags;
            CustomFontRequest fontReq = new CustomFontRequest(font, flags, numberCharsHorz, numberCharsVert);
            if (fullscreenWidth > 0)
                TCODSystem.ForceFullscrenResolution(fullscreenWidth, fullscreenHeight);

            RootConsole.Width = 80;
            RootConsole.Height = 50;
            RootConsole.WindowTitle = "tcodlib C# sample";
            RootConsole.Fullscreen = fullscreen;
            RootConsole.Font = fontReq;
            rootConsole = RootConsole.GetInstance();
            sampleConsole = RootConsole.GetNewConsole(SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT);

            setupStaticData();
            do
            {
                rootConsole.Clear();
                if (!credits)
                    credits = rootConsole.ConsoleCreditsRender(60, 42, false);
                for (int i = 0; i < sampleList.Length; i++)
                {
                    if (i == curSample)
                    {
                        // set colors for currently selected sample
                        rootConsole.ForegroundColor = (ColorPresets.White);
                        rootConsole.BackgroundColor = (ColorPresets.Blue);
                    }
                    else
                    {
                        // set colors for other samples
                        rootConsole.ForegroundColor = (ColorPresets.Gray);
                        rootConsole.BackgroundColor = ColorPresets.Black;
                    }
                    rootConsole.PrintLine(sampleList[i].name, 2, 45 - sampleList.Length + i, Background.Set, LineAlignment.Left);
                }
                rootConsole.ForegroundColor = (ColorPresets.Gray);
                rootConsole.BackgroundColor = ColorPresets.Black;
                rootConsole.PrintLine("last frame : " + ((int)(TCODSystem.LastFrameLength * 1000)).ToString() + " ms ( " + TCODSystem.FPS + "fps)", 79, 46, LineAlignment.Right);
                rootConsole.PrintLine("elapsed : " + TCODSystem.ElapsedMilliseconds + "ms " + (TCODSystem.ElapsedSeconds.ToString("0.00")) + "s", 79, 47, LineAlignment.Right);
                rootConsole.PutChar(2, 47, SpecialCharacter.ARROW_N);
                rootConsole.PutChar(3, 47, SpecialCharacter.ARROW_S);
                rootConsole.PrintLine(" : select a sample", 4, 47, LineAlignment.Left);
                rootConsole.PrintLine("ALT-ENTER : switch to " + (RootConsole.Fullscreen ? "windowed mode  " : "fullscreen mode"), 2, 48, LineAlignment.Left);

                sampleList[curSample].render(first, key);
                first = false;

                sampleConsole.Blit(0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, rootConsole, SAMPLE_SCREEN_X, SAMPLE_SCREEN_Y);

                rootConsole.Flush();
                key = Keyboard.CheckForKeypress(KeyPressType.Pressed);

                if (key.KeyCode == KeyCode.TCODK_DOWN)
                {
                    // down arrow : next sample
                    curSample = (curSample + 1) % sampleList.Length;
                    first = true;
                }
                else if (key.KeyCode == KeyCode.TCODK_UP)
                {
                    // up arrow : previous sample
                    curSample--;
                    if (curSample < 0)
                        curSample = sampleList.Length - 1;
                    first = true;
                }
                else if (key.KeyCode == KeyCode.TCODK_ENTER && key.Alt)
                {
                    // ALT-ENTER : switch fullscreen
                    RootConsole.Fullscreen = !RootConsole.Fullscreen;
                }
                else if (key.KeyCode == KeyCode.TCODK_F1)
                {
                    System.Console.Out.WriteLine("key.pressed" + " " +
                        key.LeftAlt + " " + key.LeftControl + " " + key.RightAlt +
                        " " + key.RightControl + " " + key.Shift);
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
