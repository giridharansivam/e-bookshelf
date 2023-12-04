import firebase from 'firebase/compat/app'
import 'firebase/compat/firestore'

const firebaseConfig = {
  apiKey: "AIzaSyB1aMSpKLFOBO5r1ZogW-yPnR3g1BaizFw",
  authDomain: "backend-project-ade23.firebaseapp.com",
  projectId: "backend-project-ade23",
  storageBucket: "backend-project-ade23.appspot.com",
  messagingSenderId: "926846218809",
  appId: "1:926846218809:web:086f7451f1121a254e13e8",
  measurementId: "G-S9KWX41G96"
};
firebase.initializeApp(firebaseConfig);
export default firebase;