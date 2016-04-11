/* Copyright (c) 2015  Tizian Zeltner, ETH Zurich
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
 
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

// TODO Credits to Mattia Ryffel

public class Serializer<T>
{
    private readonly string fileName;
    private string filePath {
        get { return Path.Combine(Application.persistentDataPath, fileName); }
    }

    public Serializer(string filename)
    {
        this.fileName = filename;
    }

    public void Save(T s)
    {
        XmlSerializer writer = new XmlSerializer(typeof(T));

        using (StreamWriter file = new StreamWriter(filePath)) {
            writer.Serialize(file, s);
        }

        // Debug.Log("Saved to: " + filePath);
    }

    public T Load()
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open)) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T s = (T)serializer.Deserialize(fs);

            // Debug.Log("Loaded from: " + filePath);
            return s;
        }
    }

    public bool Delete()
    {
        try {
            File.Delete(filePath);
            // Debug.Log("Deleted: " + filePath);
            return true;
        } catch {
            return false;
        }
    }
}