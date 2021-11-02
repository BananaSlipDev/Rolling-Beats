using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateUsernameDropdown : MonoBehaviour
{
    public TMP_Dropdown animal;

    public TMP_Dropdown adjetive;

    private String[] animals = {"Horse", "Duck", "Dog", "Alien", "Frog", "Axolotl", "Penguin", "Cat", "Donkey", "Goat","MAKI"} ;
    private List<String> listAnimals;

    private String[] adjetives = {"Crazy", "Horny", "Mad", "Happy", "Bisnaga", "Kinky", "Bored", "Kleptomaniac", "THE", "Silly", "Simp", "Troll", "Stubborn", "Maniac", "Skater"} ;
    private List<String> listAdjetives;
    
    // Start is called before the first frame update
    void Start()
    {
        animal.ClearOptions();
        adjetive.ClearOptions();

        animals = animals.OrderBy(x => Random.Range(0, 1000)).ToArray();
        adjetives = adjetives.OrderBy(x => Random.Range(0, 1000)).ToArray();

        listAnimals = animals.ToList();
        animal.AddOptions(listAnimals);
        

        listAdjetives = adjetives.ToList();
        adjetive.AddOptions(listAdjetives);


    }

    public void sendChoice()
    {
        string name =  adjetive.options[adjetive.value].text + animal.options[animal.value].text;
        PlayFabManager.SharedInstance.randomName = name;
        PlayFabManager.SharedInstance.generateRandomUser(name);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");

    }

    public void suffleArray()
    {
        
    }
}
