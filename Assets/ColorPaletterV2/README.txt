Welcome to the ColorPaletterV2 v1.0!

The Color Paletter is a Unity package that allows you to create and 
manage custom color palettes easily. It provides a simple and 
intuitive interface for creating, organizing, and accessing 
color palettes in your Unity projects.

=======================================
Features:
- Create custom color palettes: Design and organize your own palettes to suit your project's needs
- Create random color palettes: Generate random color palettes of any length, from 5 to an infinite amount
- Accessible through code: Use Color Paletter's class name to access and utilize color palettes programmatically in your scripts
- Simple to use: Color Paletter's user-friendly interface makes it easy to create and manage color palettes without any hassle

=======================================
Editor Usage:
	Palette Creation:
		- To access the Color Paletter Window, go to “ColorPaletterV2>Color
		Paletter” or “Window>ColorPaletterV2>Color Paletter” in the top bar
		- Create a new palette by clicking "Add Palette"
		- Expand the custom palettes section and open your new palette
		- Give the new palette a name of your choice and click "Add Color"
		- Set a color
		- Toggle "Auto" if you'd like the name to be automatically generated based
		on the inputted color
	Importing/Exporting Palettes:
		- On the palette you'd like to export, click "Get Preset String"
		- To import, click "Import" and paste in your preset string. Simple!
		- To export multiple, I suggest just creating a list of palette strings in some
		sort of document :)
		- You can also import preset palettes

	Palette Randomization:
		- Have at least one color in the list
		- Press "Random". This will generate 5 new carefully selected colors for your
		Palette
		- If you want more colors to be generated, add some new entries, then click
		"Random"

=======================================
Main Code Usage (there are other public static methods, but you shouldn't really mess with them): 
	ColorPaletter.GetColor(string colorName)
		- returns first palette color with the inputted name, null if not found
		- Color myColor = ColorPaletter.GetColor("My Color").color;

	ColorPaletter.GetColorFromPalette(string paletteName, string colorName)
		- returns first found palette color from specified palette
		- overloads:
			- (ColorPalette colorPalette, string colorName)
			
	ColorPaletter.AddNewCustomPalette(string paletteName)
		- add a new custom palette with the provided name
		- returns newly created palette
		-overloads:
			- (string paletteName, List<PaletteColor> paletteColors)
		
	ColorPaletter.GetCustomPalettes() 
		- returns a list of your custom created color palettes

=======================================
If you have any further questions, let me know at 
I hope you enjoy this package :)		
		
Color Paletter was developed by Christian








