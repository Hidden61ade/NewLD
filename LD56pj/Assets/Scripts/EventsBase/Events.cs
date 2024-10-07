using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerDiedEvents
{
    public bool skipMessage;
    public OnPlayerDiedEvents(){
        skipMessage = false;
    }
    public OnPlayerDiedEvents(bool skipMessage){
        this.skipMessage = skipMessage;
    }
}
public class OnLevelCompleteEvent
{

}
public class OnLevelResetEvent
{

}
public class OnStateChangeEvent
{

}
public class OnSceneLoadingStartEvent{

}
public class OnSceneLoadedEvent
{

}
