namespace NNNA
{
	class Unit : MovibleSprite
	{
		protected Joueur _joueur;

		protected int _life;
		public int Life
		{
			get { return _life; }
			set
			{
				_life = value;
				if (value > _maxLife)
				{ _maxLife = value; }
			}
		}
		protected int _maxLife;
		public int MaxLife
		{
			get { return _maxLife; }
			set { _maxLife = value; }
		}

		protected int _attaque;
		public int Attaque
		{
			get { return _attaque; }
			set { _attaque = value; }
		}

		protected int _portee;
		public int Portee
		{
			get { return _portee; }
			set { _portee = value; }
		}

		protected int _regeneration;
		public int Regeneration
		{
			get { return _regeneration; }
			set { _regeneration = value; }
		}

		protected int _lineSight;
		public int LineSight
		{
			get { return _lineSight; }
			set { _lineSight = value; }
		}

		public Unit(int x, int y)
			: base(x, y)
		{ }

		public void Attack(Unit obj)
		{ Destination = obj; }
	}
}
