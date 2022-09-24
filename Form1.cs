using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;

namespace SuDoKu

// Generate a 9x9 set of buttons (done)
// Give each button same event handler (done)
// Catch invalid inputs (done)
// When button is clicked, check if user input is zero then insert or delete (done)
// Store Inputs into array (done)
// Randomly populate board with 20 numbers that don't have dups in row & col. These cannot be changed by the player. rnd.Next(1, 9)

// Check if the board is a winning position (Check for duplicates in row && col) each turn 
// Send notice if a player won or lost
// clear button value  
// turn counter 

// Create dictionary to store values to check

//2d array [row , column] 
/*it will be separated like this 

[     1 2 3 4 5 6 7 8 9  
    1{0,0,0,0,0,0,0,0,0} 
    2{0,0,0,0,0,0,0,0,0} 
    3{0,0,0,0,0,0,0,0,0} 
    4{0,0,0,0,0,0,0,0,0} 
    5{0,0,0,0,0,0,0,0,0} 
    6{0,0,0,0,0,0,0,0,0} 
    7{0,0,0,0,0,0,0,0,0} 
    8{0,0,0,0,0,0,0,0,0} 
    9{0,0,0,0,0,0,0,0,0}   
                            
]
*/


{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        public class SudokuButton : Button //SudokuButton extends the properties of regular button 
        {
            public int row {
                get;
                set;
            }
            
            public int col {
                get;
                set;
            }

            public bool locked {
                get;
                set;
            } 
        }

        public SudokuButton[,]boardArray = new SudokuButton[9,9];

        private void StartGame_Click(object sender, EventArgs e)
        {
            //Generate Board Buttons & attach event handler
            for (int i = 0; i < 9; i++) {

                for (int j = 0; j < 9; j++) {

                    SudokuButton boardButton = new SudokuButton();
                    
                    boardButton.Location = new System.Drawing.Point((60 * j) + 50, (60 * i) + 50);

                    boardButton.Name = i.ToString() + j.ToString();
                    boardButton.Size = new System.Drawing.Size(60, 60);
                    boardButton.Click += new System.EventHandler(this.selectButton_Click);
                    this.Controls.Add(boardButton);
                    boardButton.row = i;
                    boardButton.col = j;
                    boardButton.BackColor = Color.White;

                    // when generating new buttons set true 
                    // if button is locked return;
                    boardButton.Text = "";
                    boardArray[i , j] = boardButton;
                }
            
            }

            //Generate random numbers that cannot change
            generateRandom(boardArray);
           
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++) {

                for (int j = 0; j < 9; j++)
                {
                    this.Controls.Remove(boardArray[i, j]);

                }
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            SudokuButton btn = (SudokuButton)sender;
            if (btn.locked) return;

            //assigns value catch error for empty/invalid box            
            try
            {
                int userInput = Int16.Parse(textBox1.Text);

                if (userInput == 0)
                {
                    boardArray[btn.row, btn.col].Text = "";
                }
                else if (userInput >= 1 && userInput <= 9)
                {
                   
                    
                    boardArray[btn.row, btn.col].Text = userInput.ToString(); 
                    var check = isValid(boardArray);
                    
                    if (!check) {
                       
                        boardArray[btn.row, btn.col].Text = "";
                        MessageBox.Show("Cannot Enter Here");                       
                    }
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid try again");
            }

        }

        public void generateRandom(SudokuButton[,] boardArray) {
            int counter = 0;
            Random rnd = new Random(); 
                
            while (counter < 20) {
                //generate random row and column
                //if equal to empty
                int row = rnd.Next(0, 9);
                int col = rnd.Next(0, 9);
                int val = rnd.Next(1, 10);

                

                if (boardArray[row, col].Text == "") {
                    boardArray[row, col].Text = val.ToString();

                    if (!isValid(boardArray))
                    {
                        boardArray[row, col].Text = "";
                    }
                    else {
                        boardArray[row,col].locked = true;
                        boardArray[row, col].BackColor = Color.LightBlue;
                        counter++;
                    }
                }
            }
        }

        public bool isValid(SudokuButton[,] boardArray) {
            //Perform Check

            //use hashset; unordered collection of unique elements
            //if value gets added
            IDictionary<int, HashSet<String>> rows = new Dictionary<int, HashSet<String>>();
            IDictionary<int, HashSet<String>> columns = new Dictionary<int, HashSet<String>>();

            //tuple / checking for block
            IDictionary<(int,int), HashSet<String>> blocks = new Dictionary<(int,int), HashSet<String>>();

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (boardArray[r, c].Text != "")
                    {

                        if (rows.ContainsKey(r))
                        {
                            if (!rows[r].Add(boardArray[r, c].Text))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            rows.Add(r, new HashSet<string>() { boardArray[r, c].Text });
                        }

                        //columns
                        if (columns.ContainsKey(c))
                        {
                            if (!columns[c].Add(boardArray[r, c].Text))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            columns.Add(c, new HashSet<string>() { boardArray[r,c].Text });
                        }

                        Console.WriteLine((r / 3, c / 3));

                        if (blocks.ContainsKey((r / 3 , c / 3)))
                        {

                            if (!blocks[(r / 3, c / 3)].Add(boardArray[r, c].Text))
                            {
                                return false;
                            }

                        }
                        else
                        {
                            blocks.Add((r / 3, c / 3), new HashSet<string>() { boardArray[r, c].Text });
                        }
                        
                    }
                }

            }

            return true;
        }

        private System.Windows.Forms.Button boardButton;

        private void Form1_Load(object sender, EventArgs e){}

    }
}