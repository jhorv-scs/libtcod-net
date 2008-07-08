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

        public void Dispose()
        {
            sampleConsole.Dispose();
            rootConsole.Dispose();
            off_secondary.Dispose();
            off_screenshot.Dispose();
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
            TCODConsoleLinePrinter.PrintLineRect(sampleConsole, "The Doryen library uses 24 bits colors, for both background and foreground.", SAMPLE_SCREEN_WIDTH / 2, 5, SAMPLE_SCREEN_WIDTH - 2, SAMPLE_SCREEN_HEIGHT - 1, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_MULTIPLY), TCODLineAlign.Center);

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
                TCODConsolePainter.DrawBox(off_secondary, 0, 0, SAMPLE_SCREEN_WIDTH / 2, SAMPLE_SCREEN_HEIGHT / 2, false, "Offscreen console");
                TCODConsoleLinePrinter.PrintLineRect(off_secondary, "You can render to an offscreen console and blit in on another one, simulating alpha transparency.", SAMPLE_SCREEN_WIDTH / 4, 2, SAMPLE_SCREEN_WIDTH / 2 - 2, SAMPLE_SCREEN_HEIGHT / 2, TCODLineAlign.Center);
            }
            if (first)
            {
                TCODSystem.SetFPS(30); // fps limited to 30
                // get a "screenshot" of the current sample screen
                TCODConsoleBliter.ConsoleBlit(sampleConsole, 0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, off_screenshot, 0, 0, 255);

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
            TCODConsoleBliter.ConsoleBlit(off_screenshot, 0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, sampleConsole, 0, 0, 255);

            // blit the overlapping screen
            TCODConsoleBliter.ConsoleBlit(off_secondary, 0, 0, SAMPLE_SCREEN_WIDTH / 2, SAMPLE_SCREEN_HEIGHT / 2, sampleConsole, off_x, off_y, off_alpha);
        }

        static TCODConsole line_bk;
        static TCOD_bkgnd_flag line_bkFlag = TCOD_bkgnd_flag.TCOD_BKGND_SET; // current blending mode
        static bool line_init = false;
        static string[] line_flagNames = {
			    "TCOD_BKGND_NONE",
			    "TCOD_BKGND_SET",
			    "TCOD_BKGND_MULTIPLY",
			    "TCOD_BKGND_LIGHTEN",
			    "TCOD_BKGND_DARKEN",
			    "TCOD_BKGND_SCREEN",
			    "TCOD_BKGND_COLOR_DODGE",
			    "TCOD_BKGND_COLOR_BURN",
			    "TCOD_BKGND_ADD",
			    "TCOD_BKGND_ADDALPHA",
			    "TCOD_BKGND_BURN",
			    "TCOD_BKGND_OVERLAY",
			    "TCOD_BKGND_ALPHA"
		    };
        void render_lines(bool first, TCOD_key key)
        {
            sampleConsole.Clear();
            /*		if ( key->vk == TCODK_ENTER || key->vk == TCODK_KPENTER ) {
                    // switch to the next blending mode
                    bkFlag++;
                    if ( (bkFlag &0xff) > TCOD_BKGND_ALPH) bkFlag=TCOD_BKGND_NONE;
                }
                if ( (bkFlag & 0xff) == TCOD_BKGND_ALPH) {
                    // for the alpha mode, update alpha every frame
                    float alpha = (1.0f+cosf(TCODSystem::getElapsedSeconds()*2))/2.0f;
                    bkFlag=TCOD_BKGND_ALPHA(alpha);
                } else if ( (bkFlag & 0xff) == TCOD_BKGND_ADDA) {
                    // for the add alpha mode, update alpha every frame
                    float alpha = (1.0f+cosf(TCODSystem::getElapsedSeconds()*2))/2.0f;
                    bkFlag=TCOD_BKGND_ADDALPHA(alpha);
                }
                if (!init) {
                    // initialize the colored background
                    for (int x=0; x < SAMPLE_SCREEN_WIDTH; x ++) {
                        for (int y=0; y < SAMPLE_SCREEN_HEIGHT; y++) {
                            TCODColor col;
                            col.r = (uint8)(x* 255 / (SAMPLE_SCREEN_WIDTH-1));
                            col.g = (uint8)((x+y)* 255 / (SAMPLE_SCREEN_WIDTH-1+SAMPLE_SCREEN_HEIGHT-1));
                            col.b = (uint8)(y* 255 / (SAMPLE_SCREEN_HEIGHT-1));
                            bk.setBack(x,y,col);
                        }
                    }
                    init=true;
                }
                if ( first ) {
                    TCODSystem::setFps(30); // fps limited to 30
                    sampleConsole.setForegroundColor(TCODColor::white);
                }
                // blit the background
                TCODConsole::blit(&bk,0,0,SAMPLE_SCREEN_WIDTH,SAMPLE_SCREEN_HEIGHT,&sampleConsole,0,0);
                // render the gradient
                int recty=(int)((SAMPLE_SCREEN_HEIGHT-2)*((1.0f+cosf(TCOD_sys_elapsed_seconds()))/2.0f));
                for (int x=0;x < SAMPLE_SCREEN_WIDTH; x++) {
                    TCODColor col;
                    col.r=(uint8)(x*255/SAMPLE_SCREEN_WIDTH);
                    col.g=(uint8)(x*255/SAMPLE_SCREEN_WIDTH);
                    col.b=(uint8)(x*255/SAMPLE_SCREEN_WIDTH);
                    sampleConsole.setBack(x,recty,col,(TCOD_bkgnd_flag_t)bkFlag);
                    sampleConsole.setBack(x,recty+1,col,(TCOD_bkgnd_flag_t)bkFlag);
                    sampleConsole.setBack(x,recty+2,col,(TCOD_bkgnd_flag_t)bkFlag);
                }
                // calculate the segment ends
                float angle = TCOD_sys_elapsed_seconds()*2.0f;
                float cosAngle=cosf(angle);
                float sinAngle=sinf(angle);
                int xo = (int)(SAMPLE_SCREEN_WIDTH/2*(1 + cosAngle));
                int yo = (int)(SAMPLE_SCREEN_HEIGHT/2 + sinAngle * SAMPLE_SCREEN_WIDTH/2);
                int xd = (int)(SAMPLE_SCREEN_WIDTH/2*(1 - cosAngle));
                int yd = (int)(SAMPLE_SCREEN_HEIGHT/2 - sinAngle * SAMPLE_SCREEN_WIDTH/2);

                // render the line
                int x=xo,y=yo;
                TCODLine::init(x,y,xd,yd);
                do {
                    if ( x>= 0 && y >= 0 && x < SAMPLE_SCREEN_WIDTH && y < SAMPLE_SCREEN_HEIGHT) {
                        sampleConsole.setBack(x,y,TCODColor::lightBlue,(TCOD_bkgnd_flag_t)bkFlag);
                    }
                } while ( ! TCODLine::step(&x,&y));

                // print the current flag
                sampleConsole.printLeft(2,2,TCOD_BKGND_NONE,"%s (ENTER to change)",flagNames[bkFlag&0xff]);
            */
        }

        void render_noise(bool first, TCOD_key key)
        {
            sampleConsole.Clear();
        }

        void render_fov(bool first, TCOD_key key)
        {
            sampleConsole.Clear();
        }

        void render_image(bool first, TCOD_key key)
        {
            sampleConsole.Clear();
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

            TCODConsoleLinePrinter.PrintLine(sampleConsole, s, 1, 1, TCODLineAlign.Left);

            TCODConsoleLinePrinter.PrintLine(sampleConsole, "1 : Hide cursor\n2 : Show cursor", 1, 10, TCODLineAlign.Left);
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
                    TCODConsoleLinePrinter.PrintLine(rootConsole, sampleList[i].name, 2, 47 - (sampleList.Length - i) * 2, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET), TCODLineAlign.Left);
                }
                rootConsole.SetForegroundColor(TCODColor.TCOD_grey);
                rootConsole.SetBackgroundColor(TCODColor.TCOD_black);
                TCODConsoleLinePrinter.PrintLine(rootConsole, "last frame : " + (TCODSystem.GetLastFrameLength() * 1000).ToString() + " ms ( " + TCODSystem.GetFPS() + "fps)", 79, 46, TCODLineAlign.Right);
                TCODConsoleLinePrinter.PrintLine(rootConsole, "elapsed : " + TCODSystem.GetElapsedMilli() + "ms " + (TCODSystem.GetElapsedSeconds().ToString("0.00")) + "s", 79, 47, TCODLineAlign.Right);
                TCODConsoleLinePrinter.PrintLine(rootConsole, TCODSpecialChar.TCOD_CHAR_ARROW_N + TCODSpecialChar.TCOD_CHAR_ARROW_S + " : select a sample", 2, 47, TCODLineAlign.Left);
                TCODConsoleLinePrinter.PrintLine(rootConsole, "ALT-ENTER : switch to " + (rootConsole.IsFullscreen() ? "windowed mode  " : "fullscreen mode"), 2, 48, TCODLineAlign.Left);

                sampleList[curSample].render(first, key);
                first = false;

                TCODConsoleBliter.ConsoleBlit(sampleConsole, 0, 0, SAMPLE_SCREEN_WIDTH, SAMPLE_SCREEN_HEIGHT, rootConsole, SAMPLE_SCREEN_X, SAMPLE_SCREEN_Y, 255);

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