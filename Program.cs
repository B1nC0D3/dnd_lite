using Microsoft.VisualBasic;
using System;

namespace dnd_lite
{
    public class Program
    {
        // Enum with all possible actions
        public enum GameActions
        {
            Shoot,
            Repair,
            BuyBullets,
            CheckCurrentBullets
        }
        static void Main()
        {
            // Getting user name for tank
            Console.WriteLine("Please enter your name to begin game!");
            string inputName = Console.ReadLine();
            string userName = String.IsNullOrEmpty(inputName) ? "User" : inputName;

            // Initializing tank's instances
            AiTank botTank = new(50, 10, "Enemy");
            BaseTank userTank = new(50 , 10, userName);

            // Initializing list of users and queue for proper turning
            // #TODO make it work to unknown count of players 
            List<IBaseTank> players = new List<IBaseTank>{ botTank, userTank };
            Queue<IBaseTank> turnOrder = new Queue<IBaseTank>(players);

            // Starting game untill last player remains
            while (turnOrder.Count == 2)
            {
                // Take player by order
                IBaseTank curPlayerTurn = turnOrder.Dequeue();

                Console.WriteLine($"{curPlayerTurn.GetName()}'s turn!");

                // Make turn by current player
                object turnResult = DoTurn(curPlayerTurn);
                string turnMessage;


                // Silly check for results of turn.
                // If returning int then it was shoot action, and we need to register dmg for other tank
                if (turnResult.GetType() == typeof(int))
                {
                    IBaseTank shootTarget = turnOrder.Dequeue();
                    turnMessage = shootTarget.TakeDamage((int)(turnResult));

                    // Checking target health, if it more than 0, then return it to queue.
                    if (shootTarget.GetCurrentHeath() >= 0)
                    {
                        turnOrder.Enqueue(shootTarget);
                    }
                }
                else
                {
                    turnMessage = (string)(turnResult);
                }
                // Write result of turn
                Console.WriteLine(turnMessage);

                // Return player to queue
                turnOrder.Enqueue(curPlayerTurn);
            }

            // Declare winner of game when cycle is over
            IBaseTank winner = turnOrder.Dequeue();
            Console.WriteLine($"{winner.GetName()} won!");
        }

        // Method for making turns. Take tank and use method which be selected
        // #TODO Read more about it, i think it can be wroten more perfectly
        static object? DoTurn(IBaseTank playerTank)
        {
            GameActions methodName = playerTank.ChooseAction();
            var method = playerTank.GetType().GetMethod(methodName.ToString());
            var methodResponse = method.Invoke(playerTank, parameters: new object[]{ });
            return methodResponse;
        }
    }
}
