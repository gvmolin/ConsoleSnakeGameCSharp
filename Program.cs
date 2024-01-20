using System.Collections.Generic;
using System.Threading;
using System;

namespace ConsoleApp1
{
    internal class Program
    {
        private static ManualResetEvent stopEvent = new(false);

        static async Task Main(string[] args)
        {
            Controls controls = new();
            Map map = new();
            Snake snake = new();


            Task renderTask = Task.Run(() =>
            {
                //Thread thread = new(() =>
                //{
                    Timer timer = new(_ =>
                    {
                        snake.moveSnake(controls.getKey());
                        map.renderMap(snake, controls.getKey());
                        
                    }, null, 0, 500);
                    
                    stopEvent.WaitOne();
                //});

                

            });

            Task controlsTask = Task.Run(() =>
            {
                controls.startListening();
            });




            await Task.WhenAny(renderTask, controlsTask);
            stopEvent.Set(); // Signal the rendering thread to stop
            await Task.WhenAll(renderTask, controlsTask);

        }

    }

    class Snake {
        private List<int[]> snakePos; // exemplo: [[3,10],[3,11],[3,12]] -> [x, y]
        public Snake() 
        {
            snakePos = [[30, 10], [30, 11], [30, 12]];
        }

        public List<int[]> getSnakePos() 
        {
            return snakePos;
        }

        public void setSnakePos(List<int[]> newPos)
        {
            this.snakePos = newPos;
        }

        public void moveSnake(ConsoleKeyInfo key) {
            snakePos.RemoveAt(0);
            int[] head = (int[]) this.snakePos.Last().Clone();
            snakePos.Add([head[0], head[1] + 1]);
            Console.WriteLine(snakePos.Count);
            
        }
    }

    class Controls
    {
        private ConsoleKeyInfo key;
        public Controls() {
            
        }

        public void startListening() {
            while (true)
            {
                ConsoleKeyInfo input = Console.ReadKey();

                if (input.Key == ConsoleKey.UpArrow || input.Key == ConsoleKey.DownArrow || input.Key == ConsoleKey.LeftArrow || input.Key == ConsoleKey.RightArrow) 
                {
                    this.key = input;
                }

            }
        }

        public ConsoleKeyInfo getKey() 
        { 
            return this.key;
        }


    }

    class Map
    {
        public int[][] mapArr;

        public Map() 
        {
            mapArr = startMap();
        }

        public int[][] getMap()
        {
            return mapArr;
        }

        public void setMap(int[][] map)
        {
            this.mapArr = map;
        }

        public void renderMap(Snake snake, ConsoleKeyInfo key)
        {
            Console.Clear();
            int[][] clonedMap = (int[][])this.mapArr.Clone();

            int[][] deepCloned = DeepClone
            List<int[]> currentSnake = snake.getSnakePos();
            Console.WriteLine(currentSnake.Count);
            foreach (var item in currentSnake)
            {
                clonedMap[item[1]][item[0]] = 1;
            }

            foreach (var item in clonedMap)
            {
                foreach (var innerItem in item) {
                    Console.Write(innerItem);
                }
                Console.Write("\n");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Last Key:" + key.Key);
        }

        private static int[][] startMap()
        {
            int[][] map = new int[20][];
            for (int i = 0; i < map.Length; i++)
            {
                map[i] = new int[100];
                for (int n = 0; n < map[i].Length; n++)
                {
                    map[i][n] = 0;
                }
            }
            return map;
        }
    }

    class Utils { 
        
    }
}
