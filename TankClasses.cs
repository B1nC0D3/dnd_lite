using static dnd_lite.Program;

namespace dnd_lite
{
    // Base class of tank
    internal class BaseTank : IBaseTank
    {

        // Multiplier for crit rolls
        private const int CritMultiplier = 2;

        private const int MaxBullets = 15;
        
        // Multiplier for anything to add (like heal or bullets)
        private const double AddMultiplier = 0.5;

        private protected string Name;

        private protected int CurHealth;

        private protected int MaxHealth;

        private protected int Armor;

        private protected int CurBullets;

        // Array from enum with possible actions
        private protected Array Actions = Enum.GetValues(typeof(GameActions));

        public BaseTank(int health, int armor, string name)
        {
            Name = name;
            MaxHealth = health;
            Armor = armor;
            CurHealth = MaxHealth;
            CurBullets = MaxBullets;
        }
        
        // Imitate dice roll
        private int GetRandomNum()
        {
            int roll = new Random().Next(1, 21);
            // If roll more than 19 thats a crit, need to multiply roll
            if (roll >= 19)
            {
                return roll * CritMultiplier;
            }
            return roll;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetCurrentHeath()
        {
            return CurHealth;
        }


        public string CheckCurrentBullets()
        {
            return $"{Name} have {CurBullets} bullets.";
        }

        public int Shoot()
        {
            return GetRandomNum();
        }

        public string Repair()
        {
            int healed_hp = (int)(GetRandomNum() * AddMultiplier);

            // Just checks if income healing more than MaxHealth
            if (CurHealth + healed_hp > MaxHealth)
            {
                CurHealth = MaxHealth;
            }
            else
            {
                CurHealth += healed_hp;
            }
            return $"{Name} healed for {healed_hp}. {Name}'s health is {CurHealth} now.";
        }

        public string BuyBullets()
        {
            int boughtBullets = (int)(GetRandomNum() * AddMultiplier);

            // Same as in healing method
            if (boughtBullets + CurBullets > MaxBullets)
            {
                CurBullets = MaxBullets;
            }
            else
            {
                CurBullets += boughtBullets;
            }
            return $"{Name} bought {boughtBullets} bullets. {Name} have {CurBullets} bullets";
        }


        public string TakeDamage(int damage)
        {   
            // Reducing incoming damage with armor, to make game more interesting
            int takenDamage = damage - Armor;

            // Check to damage always be not negative num, overwise, override to 0
            if (takenDamage < 0)
            {
                takenDamage = 0; 
            }
            CurHealth -= takenDamage;

            //Check tank health after shot, if it below zero, tank blows up
            if (CurHealth <= 0)
            {
                return $"{Name}'s Tank has been destroed!";
            }
            return $"{Name} take {takenDamage} damage. {Name}'s health is {CurHealth} now.";
        }

        public virtual GameActions ChooseAction()
        {
            Console.WriteLine("Choose your action:");

            // Write to console all possible turn actions
            for (int count = 0; count < Actions.Length; count++)
            {
                var action = Actions.GetValue(count); 
                Console.WriteLine( $"{count + 1}. {action}");
            }

            // Need this endless cycle to prevent incorrect input from user 
            while (true)
            {   
                // Wait for input and do action depends on user choice 
                string action = Console.ReadLine();
                switch (action)
                {
                    case "1":
                        // No bullets - no shots
                        // #TODO move bullets check to class
                        if (CurBullets == 0)
                        {
                            Console.WriteLine("You dont have enough ammo");
                            continue;
                        }
                        return GameActions.Shoot;
                    case "2":
                        return GameActions.Repair;
                    case "3":
                        return GameActions.BuyBullets;
                    case "4":
                        return GameActions.CheckCurrentBullets;
                    default:
                        Console.WriteLine("Choose one of valid actions!");
                        continue;
                }
            }
            
        }
    }

    // Ai class of tank with overriden ChooseAction method
    internal class AiTank : BaseTank
    {
        public AiTank(int health, int armor, string name)
            : base(health, armor, name) { }


         override public GameActions ChooseAction()
        {
            // Choose random action from enum
            // #TODO rewrite to less crumsy manner 
            Random roll = new();
            GameActions randomAction = (GameActions)Actions.GetValue(roll.Next(Actions.Length));
            // Check current bullets, for able to shoot
            if (randomAction == GameActions.Shoot && CurBullets == 0)
            {
                return GameActions.BuyBullets;
            }
            return randomAction;
        }
    }
}
