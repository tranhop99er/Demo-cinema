
import './App.css';
import Footer from './components/Footer/Footer';
import Topbar from './components/Header/TopBar';
import AppRouter from './components/Routes/AppRouter';

function App() {
  
  return (
    <div className="App">
      <Topbar />
      <AppRouter />
      <Footer />
    </div>
  );
}

export default App;
