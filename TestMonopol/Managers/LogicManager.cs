using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMonopol.Models;

namespace TestMonopol.Managers
{
    public abstract class LogicManager
    {
        // Метод для выполнения основной логики (абстрактный, реализуется в дочерних классах)
        public abstract List<Pallet> ProcessPallets();
    }
}
