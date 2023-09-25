public class UIPopup : UIBase
{
	public virtual void Init()
	{
		Managers.UI.SetCanvas(gameObject, true);
	}

	public virtual void Close()
	{
		Managers.UI.ClosePopupUI(this);
	}
}