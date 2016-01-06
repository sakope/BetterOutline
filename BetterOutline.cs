using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UGUICustom
{
	/// <summary>
	/// uGUIのtextやimage等のコンポーネントにデフォルトの物より綺麗なアウトラインをつけます。
	/// </summary>
	[AddComponentMenu("UI/Effects/BetterOutline")]
	public class BetterOutline : Shadow
	{
		//円周上にメッシュを書き足していく際の分割数
		[Range(2, 16)]
		[SerializeField] private int _divideAmoumt = 8;

		protected BetterOutline()
		{
		}

		#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
		public override void ModifyVertices(List<UIVertex> verts)
		{
			if (IsActive() == false || verts == null || verts.Count == 0)
			{
				return;
			}
		#else
		public override void ModifyMesh(VertexHelper vh)
		{
			// GameComponentとComponentのアクティブチェック
			if (IsActive() == false)
			{
				return;
			}

			List<UIVertex> verts = new List<UIVertex>();
			vh.GetUIVertexStream(verts);

			//頂点リストの有無・頂点数が0ではないか・文字が1文字じゃないかチェック.
			if (verts == null || verts.Count == 0)
			{
				return;
			}

		#endif
			int start;
			int end = 0;

			//円周上にメッシュを配置
			for (float i = 0; i <= Mathf.PI * 2; i += Mathf.PI / (float)_divideAmoumt)
			{
				start = end;
				end = verts.Count;
				ApplyShadow(verts, effectColor, start, end, effectDistance.x * Mathf.Cos(i), effectDistance.y * Mathf.Sin(i));
			}
			#if !UNITY_4_6 && !UNITY_5_0 && !UNITY_5_1
			vh.Clear();
			vh.AddUIVertexTriangleStream(verts);
			#endif
		}
	}
}