using System.Collections.Generic;
using Bfm.Diet.Core.Base;

namespace Bfm.Diet.Model
{
    public class SabitTanim : ModelBase<int>
    {
        public SabitTanim()
        {
            SabitTanimDetaylari = new HashSet<SabitTanimDetay>();
        }

        public string Adi { get; set; }
        public string Aciklama { get; set; }

        public virtual ICollection<SabitTanimDetay> SabitTanimDetaylari { get; set; }
    }
}