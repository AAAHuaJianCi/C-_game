using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player_shuxing
{
    class Player
    {
        public string PlayerName;//玩家名字
        public int PlayerLevel = 1;//等级
        public double Player_Exp = 0;//经验
        public int Player_Gold = 0;//金币
        public double Player_HP = 100;//当前HP
        public double Player_Max_HP = 100;//最大HP
        public double Player_Attack = 5;//攻击力
    }

    class diren
    {
        public string direnName;//敌人名字
        public int diren_Level = 1;//等级
        public double diren_give_Exp = 0;//经验
        public int diren_give_Gold = 0;//金币
        public double diren_HP = 100;//当前HP
        public double diren_Max_HP = 100;//最大HP
        public double diren_Attack = 5;//攻击力
    }
}
