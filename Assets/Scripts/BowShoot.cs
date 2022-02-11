using UnityEngine;

public class BowShoot : WeaponController
{
    private bool bowFire = false;                       // Flag of start/end shooting
    private bool stringInertia = false;                 // Need to return string in start position after inertia
    private float stringTime = 0f;                      // How long string was stretched
    private float stringTimeCorrection = 0f;            // Correction parametr, takes value of "stringTime", not more than 1f

    private bool noArrow = true;                        // By default there is no arrow. Flag when arrow creates

    public Transform stringPos;                         // Current string location
    private Vector3 stringStartPos;                     // Start string location
    public Transform stringEndPos;                      // Limit string location

    private Vector3 bowStartPos;                        // Start bow location

    public GameObject arrowPrefab;                      // Arrow Prefab
    public AudioClip shootAudio;                        // Bow shoot sound

    void Start()
    {
        // Define string and bow start local position
        stringStartPos = stringPos.localPosition;
        bowStartPos = this.transform.localPosition;
    }

    void Update()
    {
        // Start stretch the string
        if (Input.GetMouseButton(0) && !bowFire && !stringInertia && Inventory.s_bow_MagazineInventoryCurrent > 0)
        {
            // There is a difference between "s_shooting" and "bowFire". First show that weapon is working, second - release the string flag
            WeaponController.s_shooting = true;

            // Reset flags
            stringInertia = false;
            bowFire = false;

            // Calculating string time
            stringTime += Time.deltaTime;

            // Create new arrow with position correction about string
            if (noArrow)
            {
                GameObject newArrow = Instantiate(arrowPrefab, stringPos.position + stringPos.right / 150, stringPos.rotation);
                newArrow.transform.SetParent(stringPos);
                noArrow = false;
            }

            // String on maximum size and get up the bow
            if (stringPos.localPosition.y > stringEndPos.localPosition.y)
            {
                stringPos.localPosition += new Vector3(0, -0.001f, 0) * Bow.stringSpeed * Time.deltaTime;
                
                // Get up the bow and limit the height
                if (this.transform.localPosition.y < Bow.heightLimit)
                    this.transform.localPosition += new Vector3(0, 0.01f, 0) * Bow.stringSpeed * Time.deltaTime;
            }
        } else {
            // Return bow to start position
            if (noArrow && this.transform.localPosition.y > bowStartPos.y)
                this.transform.localPosition += new Vector3(0, -0.01f, 0) * Bow.stringSpeed * Time.deltaTime;
        }

        // Release the string, except cases when bow still shooting, arrow not ready and no arrows in inventory
        if (Input.GetMouseButtonUp(0) && !bowFire && !noArrow && Inventory.s_bow_MagazineInventoryCurrent > 0)
        {
            bowFire = true;
            stringTimeCorrection = stringTime;
            stringTime = 0;
            if (stringTimeCorrection > 1f)
                stringTimeCorrection = 1f;

            // Shoot sound
            this.transform.GetComponent<AudioSource>().PlayOneShot(shootAudio);

            // Minus ammo in inventory
            Inventory.s_bow_MagazineInventoryCurrent --;
        }
        
        // Return string to start position with inertia correction and shoot the arrow
        if (bowFire && stringPos.localPosition.y <= stringStartPos.y + 0.01f * stringTimeCorrection)
            stringPos.localPosition += new Vector3(0, 0.001f, 0) * Bow.shootSpeed * Time.deltaTime * stringTimeCorrection;
        else if (bowFire)
            ArrowShoot();

        // Return string to start position after inertia correction
        if (stringInertia && stringPos.localPosition.y > stringStartPos.y)
        {
            bowFire = false;
            stringPos.localPosition += new Vector3(0, -0.001f, 0) * Bow.shootSpeed * Time.deltaTime * stringTimeCorrection;
        } else if (stringInertia)
        {
            // Inertia finished, shooting process is finished
            stringInertia = false;
            WeaponController.s_shooting = false;
        }
    }

    void ArrowShoot()
    {
        // Signal to return string in start position
        stringInertia = true;

        // Unparent the arrow
        Transform arrow = stringPos.gameObject.transform.GetChild(0);
        arrow.parent = null;

        // Give the arrow physic body and shoot
        arrow.gameObject.AddComponent<Rigidbody>();
        arrow.GetComponent<Rigidbody>().mass = Bow.arrowMass;
        arrow.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.up * Bow.shootSpeed / 15 * stringTimeCorrection;

        noArrow = true;
    }
}