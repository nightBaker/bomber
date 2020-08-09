/*-
 * #%L
 * Codenjoy - it's a dojo-like platform from developers to developers.
 * %%
 * Copyright (C) 2018 Codenjoy
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */
using Bomberman.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Demo
{
    /// <summary>
    /// This is BombermanAI client demo.
    /// </summary>
    internal class YourSolver : AbstractSolver
    {
        public YourSolver(string server)
            : base(server)
        {            
        }

        /// <summary>
        /// Calls each move to make decision what to do (next move)
        /// </summary>
        protected override string Get(Board board)
        {
            var action = Direction.Act;
            var me = board.GetBomberman();
            var bombs = board.GetBombs();
            if (!IsSafe(board,me, bombs, out int maxTicks, out int stepsFromBomb))
            {
                action = RunAwayBomb(board,me,bombs, maxTicks);

            }
            else
            {
                if (!board.IsNear(me, Element.DESTROYABLE_WALL))
                {
                    action = FindDestroyable(board, me, bombs);
                }
                
            }

            return action.ToString();
        }

        private Direction FindDestroyable(Board board, Point me, List<Bomb> bombs)
        {
            var visitedPoints = new HashSet<Point>();
            var steps = new Queue<Step>();
            steps.Enqueue(new Step { Point = me.ShiftBottom(1), InitialDirection = Direction.Down, StepNumber = 1 });
            steps.Enqueue(new Step { Point = me.ShiftTop(1), InitialDirection = Direction.Up, StepNumber = 1 });
            steps.Enqueue(new Step { Point = me.ShiftLeft(1), InitialDirection = Direction.Left, StepNumber = 1 });
            steps.Enqueue(new Step { Point = me.ShiftRight(1), InitialDirection = Direction.Right, StepNumber = 1 });

            while (steps.Any())
            {
                var nextStep = steps.Dequeue();
                if (visitedPoints.Contains(nextStep.Point)) continue;

                visitedPoints.Add(nextStep.Point);
                if (board.GetAt(nextStep.Point) == Element.Space)
                {
                    if (!IsSafe(board, nextStep.Point, bombs, out int maxTicks, out int stepFromBomb)
                        && (maxTicks == nextStep.StepNumber))
                    {

                    }
                    else
                    {
                        if(board.IsNear(nextStep.Point, Element.DESTROYABLE_WALL))
                        {
                            return nextStep.InitialDirection;
                        }
                        
                        addAllDirections(steps, nextStep);
                    }
                }
            }

            return Direction.Act;
        }

        private bool IsSafe(Board board, Point point, List<Bomb> bombs, out int maxTicks, out int stepsFromBomb)
        {
     
            maxTicks = 0;
            stepsFromBomb = 0;

            var dangerousBomb = bombs.FirstOrDefault(bomb => bomb.Point.IsOnOneLine(point, 6 - bomb.Ticks));
            if(dangerousBomb != null)
            {
                maxTicks = dangerousBomb.Ticks;
                stepsFromBomb = dangerousBomb.Point.X == point.X
                            ? Math.Abs(dangerousBomb.Point.Y - point.Y)
                            : Math.Abs(dangerousBomb.Point.X - point.X);

                return false;
            }

            return true;
        }

        private Direction RunAwayBomb(Board board,Point me,List<Bomb> bombs, int bombTicks)
        {
            var visitedPoints = new HashSet<Point>();
            var steps = new Stack<Step>();
            steps.Push(new Step { Point = me.ShiftBottom(1), InitialDirection = Direction.Down, StepNumber = 1 });
            steps.Push(new Step { Point = me.ShiftTop(1), InitialDirection = Direction.Up, StepNumber = 1 });
            steps.Push(new Step { Point = me.ShiftLeft(1), InitialDirection = Direction.Left, StepNumber = 1 });
            steps.Push(new Step { Point = me.ShiftRight(1), InitialDirection = Direction.Right, StepNumber = 1 });

            while (steps.Any())
            {
                var nextStep = steps.Pop();
                if (visitedPoints.Contains(nextStep.Point)) continue;

                visitedPoints.Add(nextStep.Point);
                if(board.GetAt(nextStep.Point) == Element.Space)
                {
                    if(!IsSafe(board, nextStep.Point,bombs, out int maxTicks, out int stepFromBomb)
                        && ( maxTicks == nextStep.StepNumber))
                    {

                    }
                    else
                    {
                        if(bombTicks == nextStep.StepNumber)
                        {
                            return nextStep.InitialDirection;
                        }

                        addAllDirections(steps, nextStep);
                    }
                }
            }

            return Direction.Act;
        }

        private void addAllDirections(Stack<Step> steps, Step step)
        {
            steps.Push(new Step { Point = step.Point.ShiftBottom(1), InitialDirection = step.InitialDirection, StepNumber = step.StepNumber + 1 });
            steps.Push(new Step { Point = step.Point.ShiftTop(1), InitialDirection = step.InitialDirection, StepNumber = step.StepNumber + 1 });
            steps.Push(new Step { Point = step.Point.ShiftLeft(1), InitialDirection = step.InitialDirection, StepNumber = step.StepNumber + 1 });
            steps.Push(new Step { Point = step.Point.ShiftRight(1), InitialDirection = step.InitialDirection, StepNumber = step.StepNumber + 1 });
        }

        private void addAllDirections(Queue<Step> steps, Step step)
        {
            steps.Enqueue(new Step { Point = step.Point.ShiftBottom(1), InitialDirection = step.InitialDirection, StepNumber = step.StepNumber + 1 });
            steps.Enqueue(new Step { Point = step.Point.ShiftTop(1), InitialDirection = step.InitialDirection, StepNumber = step.StepNumber + 1 });
            steps.Enqueue(new Step { Point = step.Point.ShiftLeft(1), InitialDirection = step.InitialDirection, StepNumber = step.StepNumber + 1 });
            steps.Enqueue(new Step { Point = step.Point.ShiftRight(1), InitialDirection = step.InitialDirection, StepNumber = step.StepNumber + 1 });
        }

        public class Step
        {
            public Point Point { get; set; }

            public Direction InitialDirection { get; set; }

            public int StepNumber { get; set; }
        }
    }
}
