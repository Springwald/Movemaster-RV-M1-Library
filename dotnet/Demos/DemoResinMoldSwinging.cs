﻿// Movemaster RV M1 Library
// https://github.com/Springwald/Movemaster-RV-M1-Library
//
// (C) 2021 Daniel Springwald, Bochum Germany
// Springwald Software  -   www.springwald.de
// daniel@springwald.de -  +49 234 298 788 46
// All rights reserved
// Licensed under MIT License

using Movemaster_RV_M1_Library;
using System;
using System.Threading.Tasks;

namespace Demos
{
    internal class DemoResinMoldSwinging
    {
        private readonly string _comportName;
        private Position _homePosition;
        const int _horizontalNeutralAngle = 180 + 45;

        public DemoResinMoldSwinging(string comportName)
        {
            _comportName = comportName;
        }

        internal async Task Run()
        {
            using (var robot = await MovemasterRobotArm.CreateAsync(comportName: _comportName))
            {
                var done = false;

                _homePosition = robot.ActualPosition.Clone();

                robot.WriteToConsole = true;
                await robot.SetSpeed(6);
                await robot.Reset();
                await robot.MoveToHomePosition();
                await robot.UpdateActualPositionByHardware();
                _homePosition = robot.ActualPosition.Clone();
                Console.WriteLine("Home position :" + _homePosition.PositionAsString);
                if (!await robot.MoveTo(x: _homePosition.X, z: _homePosition.Z, y: _homePosition.Y, p: _homePosition.P, r: _horizontalNeutralAngle)) done = true;

                while (!done)
                {
                    //await robot.SetSpeed(6);
                    //await robot.Reset();
                    //if (!await robot.MoveTo(x: _homePosition.X, z: _homePosition.Z, y: _homePosition.Y, p: _homePosition.P, r: _horizontalNeutralAngle)) break;

                    Console.WriteLine();
                    Console.WriteLine("--------------");
                    Console.Write("Actual position :");
                    Console.WriteLine(robot.ActualPosition.PositionAsString);
                    Console.WriteLine();

                    await robot.UpdateActualPositionByHardware();

                    Console.WriteLine("Please choose a function:");

                    Console.WriteLine("1: Fill in position with top loading.");
                    Console.WriteLine("2: Fill in position with front loading.");
                    Console.WriteLine("3: swing for 30 minutes");
                    Console.WriteLine("4: swing endless");
                    Console.WriteLine();
                    Console.WriteLine("SPACE: update position display");
                    Console.WriteLine();
                    Console.WriteLine("0: Exit");

                    // if (Debugger.IsAttached) await SwingEndless(robot);

                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        done = true;
                    }
                    else
                    {
                        switch (key.KeyChar)
                        {
                            case '1':
                                await FillInPositionWithTopLoading(robot);
                                break;
                            case '2':
                                await FillInPositionWithFrontLoading(robot);
                                break;
                            case '3':
                                await SwingFor30Minutes(robot);
                                break;
                            case '4':
                                await SwingEndless(robot);
                                break;

                            case '0':
                                done = true;
                                break;
                        }
                    }

                }
            }
        }

        private async Task<bool> SwingEndless(MovemasterRobotArm robot)
        {
            const int rotationAngle = 90;
            Console.Clear();
            Console.WriteLine("Swinging endless.");
            Console.WriteLine("Press any key to stop swinging.");
            robot.RMode = MovemasterRobotArm.RModes.Absolute;
            await robot.SetToolLength(0);
            await robot.SetSpeed(4);

           // var x = 0;
           // var y = 450;
           // var z = 300;

            //if (!await robot.MoveTo(x: _homePosition.X, z: _homePosition.Z-5, y: _homePosition.Y, p: _homePosition.P, r: _horizontalNeutralAngle)) return false;

            if (!await robot.UpdateActualPositionByHardware()) return false;
            var startAngle = robot.ActualPosition.R;
            var noError = true;
           // if (!await robot.RotateAxis(x: 0, z: 0, y: 0, p: 0, r: -rotationAngle/2)) return false;

            while (!Console.KeyAvailable)
            {
                noError = false;

                // up
                if (!await robot.MoveTo(x: 0, z: 271, y: 636, p: 87, r: 100)) break;
                if (!await robot.MoveTo(x: 0, z: 321, y: 610, p: 43, r: -37)) break;
                if (Console.KeyAvailable) break;
                if (!await robot.MoveTo(x: 0, z: 321, y: 610, p: 43, r: 319)) break;
                if (Console.KeyAvailable) break;

                // down
                if (!await robot.MoveTo(x: 0, z: 334, y: 405, p: -85, r: 100)) break;
                if (!await robot.MoveTo(x: 0, z: 321, y: 510, p: -43, r: -37)) break;
                if (Console.KeyAvailable) break;
                if (!await robot.MoveTo(x: 0, z: 321, y: 510, p: -43, r: 319)) break;
                if (Console.KeyAvailable) break;

                // middle
                if (!await robot.MoveTo(x: 0, z: 321, y: 510, p: 0, r: -37)) break;
                if (Console.KeyAvailable) break;
                if (!await robot.MoveTo(x: 0, z: 321, y: 510, p: 0, r: 319)) break;
                if (Console.KeyAvailable) break;

                //// up
                //if (!await robot.MoveTo(x: 0, z: 68.5, y: 717.7, p: 155.1, r: 310)) break;
                //if (Console.KeyAvailable) break;
                //if (!await robot.RotateAxis(x: 0, z: 0, y: 0, p: 0, r: -180)) break;
                //if (Console.KeyAvailable) break;
                //if (!await robot.RotateAxis(x: 0, z: 0, y: 0, p: 0, r: 180)) break;
                //if (Console.KeyAvailable) break;

                //// center
                //if (!await robot.MoveTo(x: _homePosition.X, z: _homePosition.Z-5, y: _homePosition.Y, p: _homePosition.P, r: _horizontalNeutralAngle)) return false;
                //if (Console.KeyAvailable) break;
                //// rotate in center
                //if (!await robot.RotateAxis(x: 0, z: 0, y: 0, p: 0, r: -180)) break;
                //if (Console.KeyAvailable) break;
                //if (!await robot.RotateAxis(x: 0, z: 0, y: 0, p: 0, r: 180)) break;
                //if (Console.KeyAvailable) break;

                noError = true;
            }
            while (!Console.KeyAvailable)
            {
            }
            Console.ReadKey();
            return noError;
        }




        private Task SwingFor30Minutes(MovemasterRobotArm robot)
        {
            throw new NotImplementedException();
        }

        private Task FillInPositionWithFrontLoading(MovemasterRobotArm robot)
        {
            throw new NotImplementedException();
        }

        private Task FillInPositionWithTopLoading(MovemasterRobotArm robot)
        {
            throw new NotImplementedException();
        }
    }
}
