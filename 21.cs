using System;

namespace firstapp
{
    class Program
    {
        static void Main(string[] args)
        {
            Game Game1 = new Game();
            Gamer Gamer1 = new Gamer(1);
            Gamer Gamer2 = new Gamer(0);
            Console.Write("\n\n Gamer1:\n");
            Game.DisplayCards(Gamer1);
            Console.Write("\n\n You:\n");
            Game.DisplayCards(Gamer2);
            int i = 2;
            if (Game.Calculate(Gamer1.Cards) != 21 && Game.Calculate(Gamer2.Cards) != 21)
            {
                while (Gamer.WantMorecard().KeyChar.ToString() == "y" && i < 8 && Game.Calculate(Gamer2.Cards) < 21)
                {
                    Console.Write("\nGetting more cards\n");
                    Gamer2.Cards[i] = Game.GetCard();
                    i++;
                    Console.Write("\n\n Gamer1:\n");
                    Game.DisplayCards(Gamer1);
                    Console.Write("\n\n You:\n");
                    Game.DisplayCards(Gamer2);
                    if (Game.Calculate(Gamer2.Cards) >= 21)
                        break;
                }
                i = 2;
                while (Game.Calculate(Gamer1.Cards) < 17)
                {
                    Gamer1.Cards[i] = Game.GetCard();
                    i++;
                }
            }
            Console.Write("\n\n Gamer1:\n");
            Console.Write($"\nCard0 {Gamer1.Cards[0]}\n");
            Game.DisplayCards(Gamer1);
            Console.Write($"\nTotal: {Game.Calculate(Gamer1.Cards)}\n");
            Console.Write("\n\n You:\n");
            Game.DisplayCards(Gamer2);
            if ((Game.Calculate(Gamer1.Cards) > Game.Calculate(Gamer2.Cards) && Game.Calculate(Gamer1.Cards) < 22) || Game.Calculate(Gamer2.Cards) > 21)
                Console.Write("\n You lost!");
            else if (Game.Calculate(Gamer1.Cards) == Game.Calculate(Gamer2.Cards))
                Console.Write("\n No one win!");
            else
                Console.Write("\n You win!");
            Console.ReadKey();
        }
    }
    class Game
    {
        public static int GetCard()
        {
            int[] Cards = new int[52] { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 8, 9, 9, 9, 9, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 11, 11, 11, 11 };
            Random random = new Random();
            int Index = random.Next(0, Cards.Length);
            int Card = Cards[Index];
            return Card;
        }
        public static int Calculate(int[] Cards)
        {
            int total = 0;
            foreach (int i in Cards)
                total = total + i;
            return total;
        }
        public static void DisplayCards(Gamer GamerLocal)
        {
            for (int i = 0; i < 9; i++)
            {
                if (GamerLocal.Cards[i] > 0 && (GamerLocal.IsCas == 0 || i > 0))
                    Console.Write($"Card{i}: {GamerLocal.Cards[i]}\n");
            }
            if (GamerLocal.IsCas != 1)
                Console.Write($"\n Total: {Game.Calculate(GamerLocal.Cards)}\n");
        }
    }
    class Gamer
    {
        public int[] Cards = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static ConsoleKeyInfo Answer;
        public int IsCas;
        public static ConsoleKeyInfo WantMorecard()
        {
            Console.WriteLine("\nWant more cards? (y\\n)\n");
            Answer = Console.ReadKey();
            return Answer;
        }
        public Gamer(int Cas)
        {
            Cards[0] = Game.GetCard();
            Cards[1] = Game.GetCard();
            IsCas = Cas;
        }
    }
}