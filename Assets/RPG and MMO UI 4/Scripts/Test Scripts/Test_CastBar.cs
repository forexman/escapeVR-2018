using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
	public class Test_CastBar : MonoBehaviour {
		
		[SerializeField] private UICastBar m_CastBar;
		private UISpellInfo spell1;
		private UISpellInfo spell2;
		
		void Start()
		{
			if (this.m_CastBar != null && UISpellDatabase.Instance != null)
			{
				this.spell1 = UISpellDatabase.Instance.Get(0);
				this.spell2 = UISpellDatabase.Instance.Get(2);
				
				this.StartCoroutine("StartTestRoutine");
			}
		}
		
		IEnumerator StartTestRoutine()
		{
			yield return new WaitForSeconds(1f);
			
			this.m_CastBar.StartCasting(this.spell1, this.spell1.CastTime, (Time.time + this.spell1.CastTime));
			
			yield return new WaitForSeconds(1f + this.spell1.CastTime);
			
			this.m_CastBar.StartCasting(this.spell2, this.spell2.CastTime, (Time.time + this.spell2.CastTime));
			
			yield return new WaitForSeconds(this.spell2.CastTime * 0.75f);
			
			this.m_CastBar.Interrupt();
			
			this.StartCoroutine("StartTestRoutine");
		}
	}
}
