using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simular : MonoBehaviour
{
    public float simulationSpeed = 1.0f;
    
    public void reproducir()
    {
        StartCoroutine(RunStructuralAnalysis());
    }

    IEnumerator RunStructuralAnalysis()
    {
        List<Beam> allBeams = new List<Beam>(FindObjectsOfType<Beam>());
        
        // Paso 1: Establecer conexiones y apoyos
        foreach (Beam beam in allBeams)
        {
            beam.connectedBeams = FindConnectedBeams(beam, allBeams);
            beam.CheckSupport(allBeams);
        }

        // Paso 2: Calcular tensiones y fallas
        bool structureChanged;
        do
        {
            structureChanged = false;
            
            foreach (Beam beam in allBeams.ToArray())
            {
                if (beam.isBroken) continue;
                
                beam.CalculateRealStress();
                beam.CheckFailure();
                
                if (beam.isBroken)
                {
                    allBeams.Remove(beam);
                    structureChanged = true;
                    yield return new WaitForSeconds(1.0f/simulationSpeed);
                }
            }
            
        } while (structureChanged);
    }

    private List<Beam> FindConnectedBeams(Beam targetBeam, List<Beam> allBeams)
    {
        List<Beam> connections = new List<Beam>();
        
        foreach (Beam beam in allBeams)
        {
            if (beam == targetBeam || beam.isBroken) continue;
            
            bool sharesStartAnchor = beam.startAnchor == targetBeam.startAnchor || 
                                   beam.startAnchor == targetBeam.endAnchor;
            
            bool sharesEndAnchor = beam.endAnchor == targetBeam.startAnchor || 
                                 beam.endAnchor == targetBeam.endAnchor;
            
            if (sharesStartAnchor || sharesEndAnchor)
            {
                connections.Add(beam);
            }
        }
        
        return connections;
    }
}

