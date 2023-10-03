using System.ComponentModel;

public interface IPassiveSkill : ISkill, INotifyPropertyChanged
{
	bool IsEnabled { get; set; }

	bool HasPresentNumber { get; }

	int PresentNumber { get; }
}