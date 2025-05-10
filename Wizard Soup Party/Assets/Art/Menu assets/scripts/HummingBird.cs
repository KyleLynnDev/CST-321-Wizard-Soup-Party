using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public class HummingbirdCursor : MonoBehaviour
{
    public string framesDirectory = "Hummingbird_Menu_Parts/Frames"; // Path relative to Resources folder
    public float frameRate = 0.04f; // 25 FPS
    
    [Header("Hotspot Settings")]
    [Tooltip("The hotspot is the actual click point of the cursor")]
    public Vector2 hotspot = Vector2.zero;
    
    
    [Header("Audio Settings")]
    public AudioClip loopAudio;
    public AudioClip dashAudio;
    [Range(0f, 1f)]
    public float loopVolume = 0.5f;
    [Range(0f, 1f)]
    public float dashVolume = 0.5f;
    public float dashSpeedThreshold = 10f; // Speed threshold to trigger dash sound
    public float dashCooldown = 0.3f; // Minimum time between dash sounds

    private bool isAnimating = true;
    private Texture2D[] hummingbirdFrames;
    private int currentFrame = 0;
    private float frameTimer = 0f;
    private float dashTimer = 0f;
    private Vector3 lastPosition;
    private float lastDashTime = -Mathf.Infinity;

    // Audio sources
    private AudioSource loopAudioSource;
    private AudioSource dashAudioSource;
    
    void Start()
    {
        // Load all frames automatically
        LoadFrames();
        
        // Prevent a bogus “dash” on frame 1
        lastPosition = Input.mousePosition;

        // Setup audio sources
        SetupAudioSources();
        // Start the animation coroutine
        StartCoroutine(AnimateCursor());
    }
    
    void SetupAudioSources()
    {
        // Create audio source for loop
        loopAudioSource = gameObject.AddComponent<AudioSource>();
        loopAudioSource.clip = loopAudio;
        loopAudioSource.loop = true;
        loopAudioSource.volume = loopVolume;
        loopAudioSource.playOnAwake = true;
        loopAudioSource.Play();
        
        // Create audio source for dash
        dashAudioSource = gameObject.AddComponent<AudioSource>();
        dashAudioSource.clip = dashAudio;
        dashAudioSource.loop = false;
        dashAudioSource.volume = dashVolume;
        dashAudioSource.playOnAwake = false;
    }
    
    void LoadFrames()
    {
        // This will load all textures from Resources/Hummingbird_Menu_Parts/Frames
        hummingbirdFrames = Resources.LoadAll<Texture2D>(framesDirectory)
            .OrderBy(texture => texture.name) // Make sure frames are in correct order
            .ToArray();
            
        if (hummingbirdFrames.Length == 0)
        {
            Debug.LogError("No frames found in Resources/" + framesDirectory);
        }
        else
        {
            Debug.Log("Loaded " + hummingbirdFrames.Length + " hummingbird frames");
        }
    }
    
    void Update()
    {
        // Check for rapid movement to trigger dash sound
        CheckForDash();
        
        // Update audio volumes in case they're changed in the inspector
        loopAudioSource.volume = loopVolume;
        dashAudioSource.volume = dashVolume;        
    }
    
    void CheckForDash()
    {
        Vector3 currentPos = Input.mousePosition;
        float distance = Vector3.Distance(currentPos, lastPosition);
        float speed = distance / Time.deltaTime;
        lastPosition = currentPos;

        // Only dash if enough real time has passed
        if (speed > dashSpeedThreshold && Time.time - lastDashTime >= dashCooldown)
        {
            dashAudioSource.Play();
            lastDashTime = Time.time;
        }
    }

    // Coroutine for Unity's built-in cursor system
    IEnumerator AnimateCursor()
    {
        // Hide the system cursor
        Cursor.visible = false;
        
        while (isAnimating && hummingbirdFrames != null && hummingbirdFrames.Length > 0)
        {
            // Get current frame
            Texture2D currentTexture = hummingbirdFrames[currentFrame];
            
            // Set the cursor (using Unity's built-in system)
            Cursor.SetCursor(currentTexture, hotspot, CursorMode.Auto);
            
            // Move to next frame
            currentFrame = (currentFrame + 1) % hummingbirdFrames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }
    
    private void OnDestroy()
    {
        // Restore the system cursor when this script is destroyed
        Cursor.visible = true;
        // Stop audio sources
        if (loopAudioSource != null)
        {
            loopAudioSource.Stop();
        }
        
        if (dashAudioSource != null)
        {
            dashAudioSource.Stop();
        }
    }
}
