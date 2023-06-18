
using static dnd_lite.Program;

namespace dnd_lite
{
    // Interface for Tank instance
    public interface IBaseTank
    {

        public string GetName();

        public int GetCurrentHeath();

        public int Shoot();

        public string Repair();

        public string BuyBullets();

        public string CheckCurrentBullets();

        public string TakeDamage(int damage);

        public GameActions ChooseAction();
    }
}