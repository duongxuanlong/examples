using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : BaseUIController {

    string m_Content;
    Text m_TextComponent;

	void Awake ()
    {
        base.Awake();

        if (m_TextComponent == null)
        {
            m_TextComponent = GetComponent<Text>();
        }
    }

    void LateUpdate ()
    {
        base.LateUpdate();

    }

    public void SetText (string text)
    {
        m_Content = text;
        m_TextComponent.text = m_Content;
    }

    public void SetText(float text)
    {
        m_Content = text.ToString();
        m_TextComponent.text = m_Content;
    }
}
