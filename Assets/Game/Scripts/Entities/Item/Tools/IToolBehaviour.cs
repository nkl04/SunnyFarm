using System.Collections.Generic;

public interface IToolBehaviour
{
    void OnPress();
    void OnHold(ref bool isUisng);
    void OnRelease();
    void Use(List<GridPropertiesDetail> tileDetails);

    void Reactivate();
}
