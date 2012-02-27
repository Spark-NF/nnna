using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace NNNA
{
	class Clavier
	{
		// Début de la partie spécifique au pattern singleton //
		private static Clavier _instance;
		public static Clavier Get()
		{ return _instance ?? (_instance = new Clavier()); }

		private Clavier()
		{ }
		// Fin de la partie spécifique au pattern singleton //


		private KeyboardState _oldState, _newState;

		/// <summary>
		/// Met à jour le statut du clavier.
		/// </summary>
		/// <param name="state">Le nouveau statut du clavier.</param>
		public void Update(KeyboardState state)
		{
			_oldState = _newState;
			_newState = state;
		}

		/// <summary>
		/// Si un bouton est appuyé.
		/// </summary>
		/// <param name="button">Le bouton à tester.</param>
		/// <returns>Si le est appuyé.</returns>
		public bool Pressed(Keys button)
		{ return _newState.IsKeyDown(button); }

		/// <summary>
		/// Si un bouton vient d'être relaché.
		/// </summary>
		/// <param name="button">Le bouton à tester.</param>
		/// <returns>Si le bouton vient d'être relaché.</returns>
		public bool Released(Keys button)
		{ return _newState.IsKeyUp(button); }

		/// <summary>
		/// Si un bouton vient d'être appuyé.
		/// </summary>
		/// <param name="button">Le bouton à tester.</param>
		/// <returns>Si le bouton vient d'être appuyé.</returns>
		public bool NewPress(Keys button)
		{ return _oldState.IsKeyUp(button) && _newState.IsKeyDown(button); }

		/// <summary>
		/// Si un bouton vient d'être appuyé.
		/// </summary>
		/// <returns>Si un bouton vient d'être appuyé.</returns>
		public bool NewPress()
		{ return _newState.GetPressedKeys().Count() > _oldState.GetPressedKeys().Count(); }

		/// <summary>
		/// Si un bouton vient d'être relaché.
		/// </summary>
		/// <returns>Si un bouton vient d'être relaché.</returns>
		public bool NewRelease(Keys button)
		{ return _oldState.IsKeyDown(button) && _newState.IsKeyUp(button); }

		/// <summary>
		/// Si un bouton vient d'être relaché.
		/// </summary>
		/// <returns>Si un bouton vient d'être relaché.</returns>
		public bool NewRelease()
		{ return _newState.GetPressedKeys().Count() < _oldState.GetPressedKeys().Count(); }

		/// <summary>
		/// Si un bouton est actuellement appuyé.
		/// </summary>
		/// <returns>Si un bouton est actuellement appuyé.</returns>
		public bool Press()
		{ return _newState.GetPressedKeys().Count() != 0; }

		/// <summary>
		/// Si aucun bouton n'est actuellement appuyé.
		/// </summary>
		/// <returns>Si aucun bouton n'est actuellement appuyé.</returns>
		public bool NoPress()
		{ return !_newState.GetPressedKeys().Any(); }

		public override string ToString()
		{ return _newState.ToString(); }
	}
}
