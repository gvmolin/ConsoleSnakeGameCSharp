using System.Collections.Generic;
using System.Threading;
using System;

namespace ConsoleApp1
{
    internal class Program
    {
        private static ManualResetEvent stopEvent = new(false);
        private static int[] mapSize = [100, 20];

        static async Task Main(string[] args)
        {
            Controls controls = new();
            Map map = new(mapSize);
            Snake snake = new();


            Task renderTask = Task.Run(() =>
            {
                //Thread thread = new(() =>
                //{
                    Timer timer = new(_ =>
                    {
                        snake.moveSnake(controls.getKey());
                        map.renderMap(snake, controls.getKey());
                        
                    }, null, 0, 300);
                    
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
        public enum StatusEnum { alive, dead, feed }

        private List<int[]> snakePos; // exemplo: [[3,10],[3,11],[3,12]] -> [x, y]
        private int[] apple;
        private StatusEnum snakeStatus;
        public Snake() 
        {
            snakeStatus = StatusEnum.alive;
            snakePos = [[30, 10], [30, 11], [30, 12], [30,13], [30, 14]];
            apple = [10, 10];
        }

        public StatusEnum getSnakeStatus() { 
            return this.snakeStatus;
        }

        public List<int[]> getSnakePos() 
        {
            return snakePos;
        }

        public void setSnakePos(List<int[]> newPos)
        {
            this.snakePos = newPos;
        }

        public int[] getApple() 
        { 
            return this.apple;
        }

        public void setApple(int[] apple) 
        { 
            this.apple = apple;
        }

        public void moveSnake(ConsoleKeyInfo key) 
        {
            int[] head = (int[])this.snakePos.Last().Clone();

            if ((string.Join(",", this.apple) == string.Join(",", head))) 
            {
                this.snakeStatus = StatusEnum.feed;
            }

            if (this.snakeStatus == StatusEnum.alive)
            {
                this.snakePos.RemoveAt(0);
            }
            else if (this.snakeStatus == StatusEnum.feed) 
            {
                this.snakeStatus = StatusEnum.alive;

            }
    
            
            
            if (key.Key == ConsoleKey.UpArrow)
            {
                snakePos.Add([head[0], head[1] - 1]);
            } 
            else if (key.Key == ConsoleKey.DownArrow) 
            {
                snakePos.Add([head[0], head[1] + 1]);
            } 
            else if (key.Key == ConsoleKey.LeftArrow) 
            {
                snakePos.Add([head[0] - 1, head[1]]);
            } 
            else 
            {
                snakePos.Add([head[0] + 1, head[1]]);
            }

            

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

                if (
                    (input.Key == ConsoleKey.UpArrow && this.key.Key != ConsoleKey.DownArrow) ||
                    (input.Key == ConsoleKey.DownArrow && this.key.Key != ConsoleKey.UpArrow) ||
                    (input.Key == ConsoleKey.LeftArrow && this.key.Key != ConsoleKey.RightArrow) ||
                    (input.Key == ConsoleKey.RightArrow && this.key.Key != ConsoleKey.LeftArrow)
                ) {
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
        private int[] mapSize;

        public Map(int[] mapSize) 
        {
            this.mapSize = mapSize;
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

            setMap(startMap());
            List<int[]> currentSnake = snake.getSnakePos();
            int[] apple = snake.getApple();
            Console.WriteLine(currentSnake.Count);


            mapArr[apple[0]][apple[1]] = 8;
            foreach (var item in currentSnake)
            {
                this.mapArr[item[1]][item[0]] = 1;
            }

            foreach (var item in this.mapArr)
            {
                foreach (var innerItem in item) {
                    Console.Write(innerItem);
                }
                Console.Write("\n");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Last Key:" + key.Key);
            Console.WriteLine("Status:" + snake.getSnakeStatus());


            int[] head = (int[])snake.getSnakePos().Last().Clone();
            apple = (int[])snake.getApple();

            Console.WriteLine("Head:" + string.Join(",", head));
            Console.WriteLine("Apple:" + string.Join(",", apple));
            Console.WriteLine("Equals:" + (string.Join(",", apple) == string.Join(",", head)).ToString());
        }

        private int[][] startMap()
        {
            int[][] map = new int[this.mapSize[1]][];
            for (int i = 0; i < map.Length; i++)
            {
                map[i] = new int[this.mapSize[0]];
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
