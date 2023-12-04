// src/components/Contact.js
import React from 'react';

const Contact = () => {
  return (
    <div className="container mt-5 "style={{color:'white',paddingTop:'3rem'}}>
      <h2 style={{textAlign:'center', color: 'white', paddingBottom: '5rem'}}>Contact Us</h2>
      <form>
        <div className="mb-3">
          <label htmlFor="name" className="form-label">
            Name
          </label>
          <input type="text" className="form-control" style={{ maxWidth: '30%' }} id="name" />
        </div>
        <div className="mb-3">
          <label htmlFor="email" className="form-label">
            Email
          </label>
          <input type="email" className="form-control" style={{ maxWidth: '30%' }} id="email" />
        </div>
        <div className="mb-3">
          <label htmlFor="message" className="form-label">
            Message
          </label>
          <textarea className="form-control"style={{ maxWidth: '55%' }} id="message" rows="7"></textarea>
        </div>
        <button type="submit" className="btn btn-primary">
          Submit
        </button>
      </form>
    </div>
  );
};

export default Contact;
