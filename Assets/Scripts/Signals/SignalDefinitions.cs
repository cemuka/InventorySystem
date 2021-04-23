
using UnityEngine.EventSystems;

public class OnItemPointerEnterSignal       : Signal<ItemOrigin, int, PointerEventData> {}
public class OnItemPointerExitSignal        : Signal<ItemOrigin, int, PointerEventData> {}
public class OnItemPointerClickSignal       : Signal<ItemOrigin, int, PointerEventData> {}


public class OnItemBeginDragSignal          : Signal<ItemOrigin, int, PointerEventData> {}
public class OnItemDragSignal               : Signal<ItemOrigin, int, PointerEventData> {}
public class OnItemEndDragSignal            : Signal<ItemOrigin, int, PointerEventData> {}


public class OnShowTooltipSignal            : Signal<string> {}
public class OnHideTooltipSignal            : Signal         {}