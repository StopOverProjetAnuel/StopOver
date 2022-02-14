using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Sprite_Maker : MonoBehaviour
{
    public Texture2D[] textureArray;
    public Color[] ColorArray;
    public Texture2D Output;

    private SpriteRenderer rend;
    private Texture2D tex;


    void Start()
    {
        initiate();
        Output = tex;
        MakeSprite(tex);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            tex = MakeTexture(textureArray, ColorArray);
            Output = tex;
            MakeSprite(tex);
        }
    }
    public void initiate()
    {   //get variables & component
        rend = GetComponent<SpriteRenderer>();
        tex = MakeTexture(textureArray,ColorArray);
        //premièrement tex = MakeTexture(textureArray,ColorArray);
    }
    public void MakeSprite(Texture2D texture)
    {

        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, tex.height), Vector2.one * 0.5f);
        //création sprite (texture, rectangle (pos0, 0,longueur,largeur) pivot 0.5f,0.5f)

        rend.sprite = newSprite;
        //affiche le sprite
    }
    public Texture2D MakeTexture(Texture2D[] layers,Color[] layersColors)
    {
        //debug log pour les gens mauvais
        if (layers.Length == 0)
        {
            Debug.Log("Error: no image information in array!");
            return Texture2D.whiteTexture;
        }
        else if (layers.Length == 1)
        {
            Debug.Log("Hum... Only 1 image layer. Are you sure you want make this texture?");
            return layers[0];
        }

        //création de la texture de taille = à la première texture du array
        Texture2D newTexture = new Texture2D(layers[0].width, layers[0].height);

        //crée un array aussi long qu'il y a de pixels sur la texture 2D;
        //c'est un array de pixels noirs
        Color[] colorArray = new Color[newTexture.width * newTexture.height];

        //crée une seconde array de array de couleurs il y a autant de array que de couleurs désirés;
        Color[][] adjustedLayers = new Color[layers.Length][];

        
        for (int i = 0; i < layers.Length; i++)
        //pour chaque sprite dans l'array
        {
            if(i == 0 || layers[i].width == newTexture.width && layers[i].height == newTexture.height)
            //si i = 0 OU si longueur du sprite[i] = longueur nouveau sprite   ET   si largeur du sprite[i] = largeur du nouveau sprite
            //si i = 0 OU si la longueur du sprite actuel est égale à la taille du nouveau sprite
            {
                adjustedLayers[i] = layers[i].GetPixels();
                //l'array d'array de couleurs[i][ = pixels du sprite[i]  ]
                //i = 0 est obligatoirement vrai car évidement que le premier sprite est aussi grand que le nouveau sprite
            }
            else
            //si ça ne fait pas la même taille
            {
                int getX, getWidth, setX, setWidth;

                //if (true) ? getX = (value) : else = 0;
                getX = (layers[i].width > newTexture.width) ? (layers[i].width - newTexture.width) / 2 : 0;
                getWidth = (layers[i].width > newTexture.width) ? newTexture.width : layers[i].width;
                setX = (layers[i].width > newTexture.width) ? (newTexture.width - layers[i].width) / 2 : 0;
                setWidth = (layers[i].width < newTexture.width) ? layers[i].width : newTexture.width;


                int getY, getHeight, setY, setHeight;

                getY = (layers[i].height > newTexture.height) ? (layers[i].height - newTexture.height) / 2 : 0;
                getHeight = (layers[i].height > newTexture.height) ? newTexture.height : layers[i].height;
                setY = (layers[i].height > newTexture.height) ? (newTexture.height - layers[i].height) / 2 : 0;
                setHeight = (layers[i].height < newTexture.height) ? layers[i].height : newTexture.height;
                //si la texture[i] est plus large/long que la nouvelle texture alors les coordonées XY de leurs pivot vont diminuer de moitié de leur différence afin que les textures soient bien centrés
                //la largeur/longueur sera égale à la plus petite taille des texture



                //nouvelle liste de pixels du sprite[i]de paramètre (si dessus)
                Color[] getPixels = layers[i].GetPixels(getX, getY, getWidth, getHeight);

                if(layers[i].width >= newTexture.width && layers[i].height >= newTexture.height)
                // si la longueur du sprite[i] >= longueur nouvelle texture   ET   largeur du sprite[i] >= largeur nouvelle texture
                //si la taille du sprite[i] == à la taille de la nouvelle texture
                {
                    adjustedLayers[i] = getPixels;
                    // l'array d'array de couleur [i][ = nouvelle liste de pixels  ]
                }
                else
                // si ils ne font pas la même taille
                {
                    // la nouvelle texture est clear (resized?)
                    Texture2D sizedLayer = ClearTexture(newTexture.width, newTexture.height);
                    sizedLayer.SetPixels(setX, setY, setWidth, setHeight, getPixels);
                    adjustedLayers[i] = sizedLayer.GetPixels();
                }
            }
        }

        //for (int i = 0; i < layersColors.Length ; i++) // set color array 100% alpha
        //pour chaque couleurs demandés
        //{
            //if (layersColors[i].a < 1)
            //si la couleur[i] à de la transparance
            //{
                //retire la transparence
                //layersColors[i] = new Color(layersColors[i].r, layersColors[i].g, layersColors[i].b, 1.0f);
            //}
        //}

        for (int x = 0; x < newTexture.width; x++)
        //pour chaque pixels de long
        {
            for (int y = 0; y < newTexture.height; y++)
            // pour chaque pixels de large
            // pour chaque pixels de la nouvelle texture
            {
                // nouvel int (index de pixel) = current longueur + (current largeur * longueur) ???
                int pixelIndex = x + (y * newTexture.width);

                for (int i = 0; i < layers.Length; i++)
                //pour chaque sprite additioné
                {
                    //nouvelle couleur = pixel de l'array de l'array de pixels à l'index crée
                    Color srcPixel = adjustedLayers[i][pixelIndex];

                    if (srcPixel.r != 0 && srcPixel.a != 0 && i < layersColors.Length)

                    // si ce pixel est un peu rouge, un peu visible et le sprite actuel n'est pas out of bound de la liste de couleurs demandés
                    {
                        srcPixel = ApplyColorToPixel(srcPixel, layersColors[i]);
                        //applique sur ce pixel la couleur demandé
                    }


                    if (srcPixel.a == 1)
                    // si ce pixel n'a pas de transparance
                    {
                        colorArray[pixelIndex] = srcPixel;
                        //liste ce pixel
                    }
                    else if (srcPixel.a > 0)
                    // si ce pixel à de la transparance
                    {
                        colorArray[pixelIndex] = NormalBlend(colorArray[pixelIndex], srcPixel);
                        //liste ce pixel après un normal blend???
                    }

                }
            }
        }

        //la nouvelle texture utilise les couleurs de la liste
        newTexture.SetPixels(colorArray);
        newTexture.Apply();

        //paramètres de la nouvelle texture
        newTexture.wrapMode = TextureWrapMode.Clamp;
        newTexture.filterMode = FilterMode.Point;

        //fin =D
        return newTexture;
    }
    Color NormalBlend(Color dest,Color src)
    {
        //cherche à additionner le layer suplémentaire  avec le layer de dessous mais pour un Alpha = 1 le layer de dessous dois réduire son alpha

        float srcAlpha = src.a; 
        float destAlpha = (1 - srcAlpha) * dest.a; //le layer de dessous perd son alpha à srcA%
        Color destLayer = dest * destAlpha; 
        Color srcLayer = src * srcAlpha; 
        return destLayer + srcLayer; 
    }
    Color ApplyColorToPixel(Color pixel, Color applyColor) 
    {
        if(pixel.r == 1)
        // si le pixel est rouge jaune magenta ou blanc
        {
            //la couleur est la couleur appliqué
            return applyColor;
        }

            //sinon blend pixel par couleur appliqué
            return pixel * applyColor;
    }
    Texture2D ClearTexture(int width, int height)
    {
        //récupère taille de la texture
        Texture2D ClearTexture = new Texture2D(width, height);
        //récupère le nombre de pixel sous forme de liste
        Color[] ClearPixels = new Color[width * height];
        //noircis tous les pixels?
        ClearTexture.SetPixels(ClearPixels);

        //ceci doit surement enlever tous les pixels pour rendre la texture vierge d'ou le terme "Clear"
        return ClearTexture;
    }
}
