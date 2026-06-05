using Oculus.Haptics;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class OnGrabHaptic : MonoBehaviour
{
    public HandGrabInteractor lInteractor;
    public HandGrabInteractor rInteractor;
    Controller lController;
    Controller rController;
    public HapticClip clip;

    HapticClipPlayer player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = new HapticClipPlayer();
        player.clip = clip;
        player.amplitude = 0.4f;
        player.frequencyShift = 0.0f;
        player.isLooping = false;
        lController = Controller.Left;
        rController = Controller.Right;
        lInteractor.WhenStateChanged += args =>
        {
            if (args.NewState == InteractorState.Select && lInteractor.Candidate != null)
                player.Play(lController);
        };
        
        rInteractor.WhenStateChanged += args =>
        {
            if (args.NewState == InteractorState.Select && rInteractor.Candidate != null)
                player.Play(rController);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
