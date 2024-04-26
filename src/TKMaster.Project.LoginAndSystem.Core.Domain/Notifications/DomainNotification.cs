namespace TKMaster.Project.LoginAndSystem.Core.Domain.Notifications;

public class DomainNotification
{
    #region Properties

    public string Key { get; private set; }

    public string Value { get; private set; }

    #endregion

    #region Constructor

    public DomainNotification(string key, string value)
    {
        Key = key;
        Value = value;
    }

    #endregion
}
