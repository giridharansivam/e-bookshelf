import OpenAI from 'openai'
import React, { useState } from 'react';
import { pdfjs } from 'react-pdf';
import 'bootstrap/dist/css/bootstrap.min.css';
import axios from 'axios';
pdfjs.GlobalWorkerOptions.workerSrc = `https://unpkg.com/pdfjs-dist@${pdfjs.version}/build/pdf.worker.js`;

function PdfReader() {
  const [numPages, setNumPages] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [pdfText, setPdfText] = useState('');
  const [input1, setInput1] = useState('');
  const [input2, setInput2] = useState('');
  const [input3, setInput3] = useState('');
  const [input4, setInput4] = useState('');
  const [file, setFile] = useState(null);
  const [coverLetter, setCoverLetter] = useState('');

  const onFileChange = (e) => {
    const selectedFile = e.target.files[0];
    if (selectedFile) {
      setFile(selectedFile)
      const reader = new FileReader();
      
      reader.onloadend = async () => {
        try {
          const pdfData = new Uint8Array(reader.result);
          const text = await extractTextFromPdf(pdfData);
          setPdfText(text);
        } catch (error) {
          console.error('Error extracting text:', error.message);
        }
      };
      reader.readAsArrayBuffer(selectedFile);
    }
  };

  const extractTextFromPdf = async (data) => {
    return new Promise((resolve, reject) => {
      try {
        const text = [];
        const loadingTask = pdfjs.getDocument({ data });
        loadingTask.promise
          .then((pdf) => {
            const promises = [];

            for (let i = 1; i <= pdf.numPages; i++) {
              promises.push(pdf.getPage(i));
            }

            Promise.all(promises)
              .then((pages) => {
                const textPromises = pages.map((page) => {
                  return page.getTextContent().then((content) => content.items.map((item) => item.str).join(' '));
                });

                return Promise.all(textPromises);
              })
              .then((pageTexts) => {
                resolve(pageTexts.join(' '));
              })
              .catch((error) => {
                reject(error);
              });
          })
          .catch((error) => {
            reject(error);
          });
      } catch (error) {
        reject(error);
      }
    });
  };

  const handleSubmit = async() => {
    
      const data = {
        pdfText: pdfText,
        input1: input1,
        input2: input2,
        input3: input3,
        input4: input4,
      };
      
      try {
        
          const generateCoverLetter = async (resumeText) => {
            try {
              const APIKEY = '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';
              const response = await axios.post(
                'https://api.openai.com/v1/engines/davinci/completions',
                {
                  prompt: `Write a cover letter based on the model cover letter the company name is ${input1}. The Job description is ${input3}`,
                  max_tokens: 1000, // Adjust as needed
                  temperature: 0.4,
                },
                {
                  headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${APIKEY}`,
                    
                  },
                }
              );
              console.log(pdfText)
              console.log('API Response:', response.data);
              setCoverLetter(response.data.choices[0]?.text || 'No response from API');
            } catch (error) {
              console.error('Error generating cover letter:', error);
              setCoverLetter('Error generating cover letter');
            }
          };
        
          // Example usage
          const resumeText = 'Your resume text goes here...';
          generateCoverLetter(resumeText);
        }
       catch (error) {
        console.error('Error:', error.message);
      }

  }
  
  

  return (
    <div className="container mt-5 " style={{zIndex: 1000,paddingTop:'3rem'}}>
            <h2 style={{textAlign:'center', color: 'white', paddingBottom: '5rem'}}>Resume Builder</h2>
      <div className="row">
        <div className="col-md-6">
          <label htmlFor="fileInput" className="custom-file-upload" style={{color:'white'}}>
            <input
              type="file"
              id="fileInput"
              accept=".pdf"
              onChange={onFileChange}
            />
            Choose Files
          </label>
        </div>
      </div>
      <div className="row mt-4">
        <div className="col-md-6">
          <label htmlFor="input1"  style={{color:'white'}}>Job Position:</label>
          <input
            type="text"
            className="form-control"
            id="input1"
            value={input1}
            onChange={(e) => setInput1(e.target.value)}
          />
        </div>
        <div className="col-md-6">
          <label htmlFor="input2"  style={{color:'white'}}>Company Name :</label>
          <input
            type="text"
            className="form-control"
            id="input2"
            value={input2}
            onChange={(e) => setInput2(e.target.value)}
          />
        </div>
      </div>
      <div className="row mt-4">
        <div className="col-md-12">
          <label htmlFor="input3"  style={{color:'white'}}>Job Description :</label>
          <textarea
            className="form-control"
            id="input3"
            value={input3}
            style={{ background: 'white', padding: '10px', borderRadius: '5px', overflow: 'auto', height: '50vh' }}
            onChange={(e) => setInput3(e.target.value)}
          />

        </div>
        <div className="col-md-6">
          <label htmlFor="input4"  style={{color:'white'}}>Input 4:</label>
          <input
            type="text"
            className="form-control"
            id="input4"
            value={input4}
            onChange={(e) => setInput4(e.target.value)}
          />
        </div>
      </div>
      <div className="row mt-4">
        <div className="col-md-12">
          <button className="btn btn-primary" onClick={handleSubmit}>
            Submit
          </button>
        </div>
      </div>
      <div>
      <div className="row mt-4">
      <div className="col-md-12" style={{ height: '100vh' }}>
        <h2 style={{ color: 'white', textAlign: 'center' }}>Generated Cover letter</h2>
        <div className="col-md-12">
          <textarea
            className="form-control"
            id="coverletter"
            value={coverLetter}
            style={{ background: 'white', padding: '10px', borderRadius: '5px', overflow: 'auto', height: '100vh' }}
          />

        </div>
      </div>
    </div>

    </div>
    </div>
  );
}

export default PdfReader;
