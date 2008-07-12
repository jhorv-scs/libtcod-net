Linux:
	Rename libtcodWrapper.dll.linux to libtcodWrapper.dll.
	LD_LIBRARY_PATH=.:${LD_LIBRARY_PATH} mono Demo.exe

Windows:
	Rename libtcodWrapper.dll.windows to libtcodWrapper.dll
	Demo.exe