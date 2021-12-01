using UnityEngine.UI;

namespace Game.UI {
	public class EmptyGraphic : Graphic {
		protected override void OnPopulateMesh(VertexHelper vh) {
			vh.Clear();
		}
	}
}

