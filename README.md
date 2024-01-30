PROG7312 POE Application
Jo Saul ST10081839

============================================================
Application Type: WPF(.Net Framework)
Traget Framework: .Net Framework 4.8
Visual Studio Version: 2022
Github Link: https://github.com/Jo-Saul/PROG7312_POE_Code/tree/master


Summary
------------------
This is a program that aims to help users learn the dewi decimal system. The application will
include three games, each with differnt activities. Currently one the first is implemented.


Running the Application
--------------------------
To run the application, run the PROG7312_POE.exe application file. Please ensure this file is
in the bin/debug folder as it needs access to the application's assests.
Or run the PROG7312_POE shortcut.


How To Use
------------
The application opens on the home screen. Here the user can navigate to the three differnt games.
Only the first one is currently implemented. In the top right is a button to exit the application.

Game UI
-----------
In the top right are the UI bottons. The help button shows instructions for the page, the pause 
button will pause the timer. The home button will lead back to the home screen and the X will 
close the application.  

Once an activity is complete a score window will aprear. This will show the score, time taken 
and lives left. At the bottom of the window, are a home button and a replay button, that will 
reset the activity. The X in the top right will close the application. 


Replacing Books
-------------------
The Replacing Books game will start with a countdown, after which the timer will start.
To play the game, drag the books from the bottom row onto the top row of the shelf. 
Once complete press the Done button in the right corner. If not all books are in the shelf,
the row will flash. If the books are all in the self, but in the wrong order, a life will be lost. 
If all three lives are lost, it is game over. 


Identifying Areas
---------------------
The Identifying Areas game will start with a countdown, after which the timer will start.
There will be four books on the left side of the screen. They will either have call numbers
or descriptions. On the right side of the screen are seven drawers that will be labeled with
either call numbers or desriptions. 

To play the game, drag the books to the correct drawer. For example, if the book has '800' on 
it, it belongs in the drawer labelled 'Literature', and vice versa. Once complete press the
Done button in the right corner.If not all books are in the drawer, the stack of books will
flash. If the books are all in the drawer, but they do not match the lables on the drawer, 
a life will be lost. If all three lives are lost, it is game over.


Finding Call Numbers
---------------------
The Finding Call Numbers game will start with a countdown, after which the timer will start.
There will be 4 holders with top level dewey decimal optioins, as well as a book with a 
description. 

To play the game, place the book in the corrrect holder according the the sepected dewey 
category of the of the description of the book. For example, if the description on the book 
states "Colour" and the four optionis are "000 General, 400 Language, 700 Arts & Recreation
and 800 Literature", the correct holder would be the one labeled "700 Arts & Recreation".

If the correct option is chosen, the holders will show lables containing options on the next
dewey level. For example, will first display "700" then it will display "750". This will 
continue until the 3rd level call number of the description is found. In this example, that
would be "752 Colour". If the incorrect option is chosen, a live will be lost. If all three 
lives are lost, it is game over.



Program Breakdown
----------------------------------------

BitmapLibrary 
	Custom class library used for creating bitmaps. Allows the creation of a bitmap
	from apng at a given filepath location. 

DeweyLibrary
	Custom class library used for getting dewey decimal data.
	DeweyDecimal
		Class that contains methods for finding, etting dewey decimal data

	DeweyDecimalClass
		Class for creating objects of dewey decimal entries

	RBDeweyTree
		Class for creating a red black tree for storing dewey items in memory
	
PROG7312_POE - WPF Application (Main Application)
	
	Classes - folder containing all classes used

		Calculator
			Class used for caluclations in application, such as calculating scores
			and checking answesrs. Has seperate methods for each game. 

		FilePaths 
			Class for holding files paths to the assests used in the application. 
			Inclues 3D objects, images and sound files. 

		TextHandler 
			Class for adding text to images. Contains differnt methods for each game. 
			Used to add text to the images used as the texture for the books and the 
			lables of the drawer. 

		Slot Classes - sub folder containg classes used to create slots

			
			DrawerSlot
				Class for drawer slots. Used for the drawer slots in the 
				Identifying Areas game. 

			DrawerSlot
				Class for holder slots. Used for the holder slots in the 
				Finding Call Numbers game. 

			ShelfSlot
				Class for ShelfSlot objects. Used for the slots of the replacing 
				books game. 

	Game Windows - folder containing windows used for each game
		
		FindingClassNumbers 
			Window for finding call numbers game. Initialises UIUserControl for the 
			management of UI components (buttons, timer, hearts). Window create the 
			visuals required, sets up the movement mechanics and populate the 
			required data. 

		IdentifyingAreas 
			Window for identifiying areas game. Initialises UIUserControl for the 
			management of UI components (buttons, timer, hearts). Window create the 
			visuals required, sets up the movement mechanics and populate the required 
			data. 
		ReplacingBooks
			Window for replacing books game. Initialises UIUserControl for the management
			of UI components (buttons, timer, hearts). Window create the visuals required, 
			sets up the movement mechanics and populate the required data. 
	
	Shared Windows - folder for windows called in mutiple other areas

		CountDownWindow 
			Window for displaying countdown. Will show a count down from 3 at the beginning 
			of each game. Handles the animation for the countdown. 

		ErrorWindow
			Window for displaying application errors. Called in try catch of other methods.

		HelpWindow 
			Window for showing instructions to the user about their current activity. Will 
			show diffrent instructions depending on current game. 

		PauseWindow 
			Window displayed when paused button is pressed. 
	

		ScoreWindow
			Window displayed when game is completed. Can either be in a visctory sate or 
			defeat state. Will show the user score, time taken and hearts left. The score 
			window has the option to either go to the game window, replay the game or close 
			the application. 

	StartUp
		Initial window. Shows the diffent games avalible. 

	UIUserControl 
		UserControl used for the management of UI elements. Includes the hearts, timer and 
		UI buttons (exit, home, pause, help). Also contains the background sound player. 
		It is used in all gameswith methods that are called to adjust the elements to 
		the specific game. 
