using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {

        Game game;
        private int score = 0;
        Timer t;
        private const int GAMESPEED = 700;
        
        //some variables initialized that I think we may need at some point, definitely up in the air whether or not we need them currently
        //private int rowCount = 0;
        //private int columnCount = 0;
        private int leftPos = 0;
        private int downPos = 0;
        //private int currentTetrominoWidth;
        //private int currentTetrominoHeigth;
        //private int currentShapeNumber;
        //private int nextShapeNumber;
        //private int tetrisGridColumn;
        //private int tetrisGridRow;
        private int rotation = 0;
        //private bool gameActive = false;
        //private bool nextShapeDrawn = false;
        //private int[,] currentTetromino = null;
        private bool rotated = false;
        private bool collideBottom = false;
        private bool collideLeft = false;
        private bool collideRight = false;
       // private bool gameOver = false;
        private int gameSpeed;
       // private double gameSpeedCounter = 0;
        private static Color O_TetrominoColor = Color.GreenYellow;
        private static Color I_TetrominoColor = Color.Red;
        private static Color T_TetrominoColor = Color.Gold;
        private static Color S_TetrominoColor = Color.Violet;
        private static Color Z_TetrominoColor = Color.DeepSkyBlue;
        private static Color J_TetrominoColor = Color.Cyan;
        private static Color L_TetrominoColor = Color.LightSeaGreen;
        //List<int> currentTetrominoRow = null;
        //List<int> currentTetrominoColumn = null;

        string[] tetrominoArray = { "","O_Tetromino" , "I_Tetromino_0",
                                        "T_Tetromino_0","S_Tetromino_0",
                                        "Z_Tetromino_0","J_Tetromino_0",
                                        "L_Tetromino_0"
                                   };
        //tetromino shapes put in the form code - was playing around with putting them in here as opposed to a seperate class. not necessarily what we'll do, was just trying it out
        #region Tetromino Shapes
        public int[,] O_Tetromino = new int[2, 2] { { 1, 1 },  // * *
                                                    { 1, 1 }}; // * *

        //---- I Tetromino------------
        public int[,] I_Tetromino_0 = new int[2, 4] { { 1, 1, 1, 1 }, { 0, 0, 0, 0 } };// * * * *

        public int[,] I_Tetromino_90 = new int[4, 2] {{ 1,0 },   // *  
                                                       { 1,0 },  // *
                                                       { 1,0 },  // *
                                                       { 1,0 }}; // *
        //---- T Tetromino------------
        public int[,] T_Tetromino_0 = new int[2, 3] {{0,1,0},    //    * 
                                                     {1,1,1}};   //  * * *

        public int[,] T_Tetromino_90 = new int[3, 2] {{1,0},     //  * 
                                                      {1,1},     //  * *
                                                      {1,0}};    //  *  

        public int[,] T_Tetromino_180 = new int[2, 3] {{1,1,1},  // * * *
                                                       {0,1,0}}; //   * 

        public int[,] T_Tetromino_270 = new int[3, 2] {{0,1},    //   * 
                                                       {1,1},    // * *
                                                       {0,1}};   //   *  
        //---- S Tetromino------------
        public int[,] S_Tetromino_0 = new int[2, 3] {{0,1,1},    //   * *
                                                     {1,1,0}};   // * *

        public int[,] S_Tetromino_90 = new int[3, 2] {{1,0},     // *
                                                      {1,1},     // * *
                                                      {0,1}};    //   *
        //---- Z Tetromino------------
        public int[,] Z_Tetromino_0 = new int[2, 3] {{1,1,0},    // * *
                                                     {0,1,1}};   //   * *

        public int[,] Z_Tetromino_90 = new int[3, 2] {{0,1},     //   *
                                                      {1,1},     // * *
                                                      {1,0}};    // *
        //---- J Tetromino------------
        public int[,] J_Tetromino_0 = new int[2, 3] {{1,0,0},    // * 
                                                     {1,1,1}};   // * * *

        public int[,] J_Tetromino_90 = new int[3, 2] {{1,1},     // * * 
                                                      {1,0},     // *
                                                      {1,0}};    // * 

        public int[,] J_Tetromino_180 = new int[2, 3] {{1,1,1},  // * * * 
                                                       {0,0,1}}; //     *

        public int[,] J_Tetromino_270 = new int[3, 2] {{0,1},    //   * 
                                                       {0,1},    //   *
                                                       {1,1 }};  // * *

        //---- L Tetromino------------
        public int[,] L_Tetromino_0 = new int[2, 3] {{0,0,1},    //     * 
                                                     {1,1,1}};   // * * *

        public int[,] L_Tetromino_90 = new int[3, 2] {{1,0},     // *  
                                                      {1,0},     // *
                                                      {1,1}};    // * *

        public int[,] L_Tetromino_180 = new int[2, 3] {{1,1,1},  // * * * 
                                                       {1,0,0}}; // *

        public int[,] L_Tetromino_270 = new int[3, 2] {{1,1},    // * * 
                                                       {0,1},    //   *
                                                       {0,1 }};  //   *
        #endregion

        public Form1()
        {
            InitializeComponent();
            this.Shown += Form1_Shown;
            this.Paint += Form1_Paint;
            this.blocksDataGrid.DefaultCellStyle.SelectionBackColor = Color.Black;//seetting the select color to black for the selection of cells - makes them invisible when selected
            this.blocksDataGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.nextGridView.DefaultCellStyle.SelectionBackColor = Color.Black;
            this.nextGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

            gameSpeed = GAMESPEED;
            KeyDown += MainWindow_KeyDown;

            
        }

        private void setupDataGridView(DataGridView grid)
        {
            grid.BackgroundColor = Color.Black;
            grid.ShowEditingIcon = false;
            grid.ShowCellToolTips = false;
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersVisible = false;
            grid.AllowUserToResizeRows = false;
            grid.AllowUserToResizeColumns = false;
            grid.ShowCellToolTips = false;
        }

        private void addColumns(DataGridView grid, int columns)
        {
            grid.ColumnCount = columns;
            for (int i = 0; i < columns; i++)
            {
                grid.Columns[i].Width = 20;
            }
        }

        private void addRows(DataGridView grid, int rows)
        {
            for (int i = 0; i < rows; i++)
            {
                grid.Rows.Add();
                grid.Rows[i].Height = 20;
                for (int j = 0; j < grid.Rows[i].Cells.Count; j++)
                {
                    grid.Rows[i].Cells[j].Style.BackColor = Color.Black;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setupDataGridView(blocksDataGrid);
            addColumns(blocksDataGrid, 10);
            addRows(blocksDataGrid, 23);

            setupDataGridView(nextGridView);
            addColumns(nextGridView, 5);
            addRows(nextGridView, 5);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            score = 0;
            lblScore.Text = score.ToString("000000");
            game = new Game(blocksDataGrid);
            game.start();
            //starting the timer
            tmr.Start();
            //calls the tmr_tick() method every time the timer ticks, will eventually be what makes the tetrominos move down every timer tick
            tmr.Tick += tmr_tick;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            blocksDataGrid.CurrentCell = null;
            blocksDataGrid.ShowCellToolTips = false;
            nextGridView.CurrentCell = null;
            nextGridView.ShowCellToolTips = false;
        }
        //draws a silver box around the datagridview and the start button. mostly experimenting with painting, could be removed
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Point[] points = { blocksDataGrid.Location, new Point (blocksDataGrid.Left, blocksDataGrid.Bottom), new Point (blocksDataGrid.Right, blocksDataGrid.Bottom),
                new Point (blocksDataGrid.Right, blocksDataGrid.Top), new Point (blocksDataGrid.Left, blocksDataGrid.Top)};
            Pen silver = new Pen(Color.Silver, 2);
            e.Graphics.DrawLines(silver, points);
            e.Graphics.DrawLine(silver, blocksDataGrid.Right + 5, blocksDataGrid.Bottom - 125, blocksDataGrid.Right + 5, blocksDataGrid.Bottom + 1); //left
            e.Graphics.DrawLine(silver, blocksDataGrid.Right + 5, blocksDataGrid.Bottom - 125, blocksDataGrid.Right + 120, blocksDataGrid.Bottom - 125); //top
            e.Graphics.DrawLine(silver, blocksDataGrid.Right + 5, blocksDataGrid.Bottom, blocksDataGrid.Right + 120, blocksDataGrid.Bottom); //bottom
            e.Graphics.DrawLine(silver, blocksDataGrid.Right + 120, blocksDataGrid.Bottom - 125, blocksDataGrid.Right + 120, blocksDataGrid.Bottom + 1); //right
        }
        //an increment score method to increment score later
        private void incrementScore (int newPoints)
        {
            score += newPoints;
            lblScore.Text = score.ToString("00000");
        }
        //the framework for what will be moving the pieces down with the arrow keys - currently has some methods that we don't have implemented yet (obviously)
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {

            if (!tmr.Enabled) { return; }
            switch (e.KeyCode.ToString())
            {
                case "Up":
                    rotation += 90;
                    if (rotation > 270) { rotation = 0; }
                    shapeRotation(rotation);
                    break;
                case "Down":
                    downPos++;
                    break;
                case "Right":
                    // Check if collided
                    TetroCollided();
                    if (!collideRight) { leftPos++; }
                    collideRight = false;
                    break;
                case "Left":
                    // Check if collided
                    TetroCollided();
                    if (!collideLeft) { leftPos--; }
                    collideLeft = false;
                    break;
            }
            moveShape();
        }
        private void shapeRotation(int _rotation)
        {

        }
        private void tmr_tick(object sender, EventArgs e)
        {

        }
        private void TetroCollided()
        {

        }
        private void checkCollided()
        {

        }
        private void moveShape()
        {

        }
    }
}
