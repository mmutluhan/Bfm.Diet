using Bfm.Diet.Core.Base;

namespace Bfm.Diet.Model
{
    public class SabitTanimDetay : ModelBase<int>
    {
        public int SabitTanimId { get; set; }
        public string Kodu { get; set; }
        public string Aciklamasi { get; set; }
        public string OzelKod { get; set; }

        public virtual SabitTanim SabitTanim { get; set; }
    }
}