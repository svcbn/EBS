public class UIScene : UIBase
{
	public virtual void Init()
	{
		Managers.UI.SetCanvas(gameObject, false);
	}
}