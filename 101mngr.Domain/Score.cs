namespace _101mngr.Domain
{
    public struct Score
    {
        public Score(int home, int away)
        {
            Home = home;
            Away = away;
        }

        public int Home { get; }

        public int Away { get; }

        public Score HomeIncrement() => new Score(Home + 1, Away);
        public Score AwayIncrement() => new Score(Home, Away + 1);
    }
}