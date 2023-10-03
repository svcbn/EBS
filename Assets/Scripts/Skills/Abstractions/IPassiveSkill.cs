using System.ComponentModel;

public interface IPassiveSkill : ISkill, INotifyPropertyChanged
{
	bool IsEnabled { get; }

	bool HasPresentNumber { get; }

	int PresentNumber { get; }
}