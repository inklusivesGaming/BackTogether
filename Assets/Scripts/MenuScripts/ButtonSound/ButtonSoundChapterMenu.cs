using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundChapterMenu : ButtonSound
{
    public GameAudioManager.ChapterMenuSounds mChapterMenuSound;

    public override void OnSelect(BaseEventData eventData)
    {
        if (!mGameAudioManager)
            return;
        mGameAudioManager.PlayMenuSound(mChapterMenuSound);
    }
}
