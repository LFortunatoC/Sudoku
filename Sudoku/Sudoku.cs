using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public class Sudoku
    {
        private int[,] firstSet =   {{9,6,1,4,3,8,7,5,2},
                                     {5,2,8,1,9,7,6,3,4},
                                     {7,3,4,6,5,2,8,1,9},
                                     {8,5,6,9,7,1,2,4,3},
                                     {4,1,3,2,8,6,5,9,7},
                                     {2,9,7,3,4,5,1,8,6},
                                     {6,4,2,8,1,9,3,7,5},
                                     {1,7,9,5,6,3,4,2,8},
                                     {3,8,5,7,2,4,9,6,1}
                                    };

        private int[,] secondSet =  {{6,8,5,1,4,2,3,7,9},
                                     {9,7,1,8,5,3,9,7,6},
                                     {2,4,3,9,7,6,5,1,8},
                                     {4,9,6,5,3,8,1,2,7},
                                     {8,5,7,2,1,4,9,6,3},
                                     {1,3,2,7,6,9,8,5,4},
                                     {7,6,8,3,2,1,4,9,5},
                                     {5,2,9,4,8,7,6,3,1},
                                     {3,1,4,6,9,5,7,8,2}
                                    };

        private int[,] gameSet = new int[9, 9];
        private int[,] gameRes = new int[9, 9];
        private int[,] gameAnswer = new int[9, 9];

        public int[,] GameSet { get => gameSet;}
        public int[,] GameAnswer { get => gameAnswer; }
        public int[,] GameRes { get => gameRes; set => gameRes = value; }

        public bool IsValidInput(Int32 num, Int32 row, Int32 column)
        {
            // First check for occurence of num in current row and column
            for(int i =0; i <9;i++)
            {
                // Dont need to check the position what the number is inserted, but the column and row of the given position
                if ((i!= row && gameRes[i,column]==num) ||(i != column && gameRes[row,i] == num)) // // Skip candidate position
                    return false;
            }

            // Then check blocks in Blocks B1...B9
            // B1=[0,0] ~ [2,2] ----B2 [0,3]~[2,5] -- B3 [0,6]~[2,8]
            // B4=[3,0] ~ [5,2] ----B5 [3,3]~[5,5] -- B6 [3,6]~[5,8]
            // B7=[6,0] ~ [8,2] ----B8 [6,3]~[8,5] -- B9 [0,6]~[2,8]

            Int32 blockRow = (row/3)*3;        // Start row of the block to be checked
            Int32 blockColumn = (column/3)*3; // Start column of the block to be checked
            for (int rw = blockRow; rw < blockRow + 3; rw++)
            {
                for (int col = blockColumn; col < blockColumn +3; col++)
                {
                    // Dont need to check the position what the number is inserted, but the column and row of the given position
                    if ((row !=rw && column!= col) &&(gameRes[rw,col] == num))   // Skip candidate position
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void GenerateGame(int gameLevel)
        {
            Random random = new Random();
           
            if ((random.Next(1, 11) %2) == 0)
            {
                gameAnswer = firstSet; 
            }
            else
            {
                gameAnswer = secondSet;
            }
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    gameSet[row, col]=0;
                }
            }
            
            Shuffle(random.Next(1, 10));
            ShowClues(gameLevel);
            gameRes = gameSet;

           

        }

        public List<int> GetPossibleNums(int row, int col)
        {
            List<int> lstGoodNumbers = new List<int>();
            for (int num = 1; num <= 9; num++)
            {
                if (IsValidInput(num, row, col))
                {
                    lstGoodNumbers.Add(num);
                }
            }
            return lstGoodNumbers;
        }

        public void EnterValue(int value,int row, int col)
        {
            gameRes[row, col] = value;
        }

        public bool IsGameFinished()
        {
            int total = 0;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (gameRes[row, col] == 0) { return false; }
                    if (!(IsValidInput(gameRes[row, col], row, col))) { return false; }
                    total += gameRes[row, col];
                }
            }
            if (total != 405) { return false; };
            return true;
        }

        private void Shuffle(int shuffesNum)
        {
            //Relabelling entries;
            //Reflection;
            //Rotation;
            //Permutation of blocks of columns 1-3, 4-6 and 7-9;
            //Permutation of blocks of rows 1-3, 4-6 and 7-9;
            //Permutation of columns 1-3;
            //Permutation of rows 1-3;
            //Permutation of columns 4-6;
            //Permutation of rows 4-6;
            //Permutation of columns 7-9;
            //Permutation of rows 7-9.

            int count = 0;
            Random random = new Random();
            int shuffle = 0;
            int lastShuffle = 0;
            do
            {

                shuffle = random.Next(1, 11);
                if (shuffle == lastShuffle)
                {
                    shuffle = (shuffle == 10) ? 0 : shuffle++;

                }

                switch (shuffle)
                {
                    case 1: // Reflection;
                        Reflect();
                        break;

                    case 2: // Rotation;
                        Rotate();
                        break;

                    case 3: // Permutation of blocks of columns 1-3, 4-6 and 7-9;;
                        PermuteColumnBlocks();
                        break;

                    case 4: // Permutation of blocks of rows 1-3, 4-6 and 7-9;;
                        PermuteRowBlocks();
                        break;

                    case 5: // Permutation of columns 1-3;
                        PermuteCol(0, 2);
                        break;

                    case 6: // Permutation of rows 1-3;
                        PermuteRow(0, 2);
                        break;

                    case 7: // /Permutation of columns 4-6;
                        PermuteCol(3, 5);
                        break;

                    case 8: // Permutation of rows 4-6;
                        PermuteRow(3, 5);
                        break;

                    case 9: // Permutation of columns 7-9;
                        PermuteCol(6, 8);
                        break;

                    case 10: // Permutation of rows 7-9.
                        PermuteRow(6, 8);
                        break;

                    default:
                        break;
                }
                count++;
            } while (count < shuffesNum);
            /*Console.WriteLine("==========\n");
            //for (int row = 0; row < 9; row++)
            //{
            //    for (int col=0;col<9; col++)
            //    { 
            //    Console.Write(gameSet[row,col]+ "  "); // Prints "Car"
            //    }
            //    Console.WriteLine("/n");
            //}*/
        }

        private void ShowClues(int gameLevel)
        {
            int numOfClues;
            Random random = new Random();
            int row = 0;
            int col = 0;
            switch (gameLevel)
            {
                case 0: // Very Easy
                    numOfClues = 39;
                    break;

                case 1: // Easy 
                    numOfClues = 37;
                    break;

                case 2: // Medium
                    numOfClues = 34;
                    break;

                case 3: // Hard
                    numOfClues = 31;
                    break;

                case 4: //Very Hard
                    numOfClues = 27;
                    break;

                default:
                    numOfClues = 37;
                    break;
            }

            for (int count = 0; count < 9;)
            {
                row = random.Next(1, 9);
                col = random.Next(1, 9);
                if (gameSet[row, col] == 0)
                {
                    gameSet[row, col] = gameAnswer[row, col];
                    gameSet[8 - row, 8 - col] = gameAnswer[8 - row, 8 - col];
                    count++;
                }
            }

            for (int count = 19; count <= numOfClues;)
            {
                row = random.Next(1, 9);
                col = random.Next(1, 9);
                if (gameSet[row, col] == 0)
                {
                    gameSet[row, col] = gameAnswer[row, col];
                    count++;
                }
            }


        }
        private void Reflect()
        {
            int[,] temp = new int[9, 9];
            if (DateTime.Now.Millisecond % 2 == 0)
            {
                //Horizontal
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        temp[row, 8-col] = gameAnswer[row, col];
                    }
                }
            }
            else
            {
                //Vertical
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        temp[8 - row, col] = gameAnswer[row, col];
                    }
                }
            }
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    gameAnswer[row, col] = temp[row, col];
                }
            }

        }
        private void Rotate()
        {
          int[,] temp = new int[9, 9];
            if (DateTime.Now.Millisecond % 2 == 0)
            {
                //ClockWise
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        temp[col, 8 - row] = gameAnswer[row, col];
                    }
                }
            }
            else
            {
                //Conterclockwise
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        temp[8 - col, row] = gameAnswer[row, col];
                    }
                }
            }

            for(int row=0;row<9;row++)
            {
               for(int col=0;col<9;col++)
                {
                    gameAnswer[row, col] = temp[row, col];
                }
            }
        }
        private void PermuteColumnBlocks()
        {
            int[] line = new int[3];
            for (int count = 0; count < 9; count++)
            {
                for (int i = 0; i < 3; i++)
                {
                    line[i] = gameAnswer[count, i];
                    gameAnswer[count, i] = gameAnswer[count, i + 6];
                    gameAnswer[count, i + 6] = line[i];
                }

            }
        }
        private void PermuteRowBlocks()
        {
            int[] row = new int[3];
            for (int count = 0; count < 9; count++)
            {
                for (int i = 0; i < 3; i++)
                {
                    row[i] = gameAnswer[i,count];
                    gameAnswer[i,count] = gameAnswer[ i + 6,count];
                    gameAnswer[i+6,count] = row[i];
                }

            }
        }
        private void PermuteCol(int col1, int col2)
        {
            int temp;
            for (int count = 0; count < 9; count++)
            {
                temp = gameAnswer[count, col2];
                gameAnswer[count, col2] = gameAnswer[count,col1];
                gameAnswer[count,col1] = temp;
            }
        }
        private void PermuteRow(int row1, int row2)
        {
            int temp;
            for (int count = 0; count < 9; count++)
            {
                temp = gameAnswer[row2, count];
                gameAnswer[row2, count] = gameAnswer[row1, count];
                gameAnswer[row1, count] = temp;
            }
        }
        
    }
}
