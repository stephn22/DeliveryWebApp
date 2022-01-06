import './App.css';
import Header from './components/header/Header';

const sections = [
  {
    title: 'Home',
    url: '/'
  },
  {
    title: 'About',
    url: '/about'
  }
];

function App() {
  return (
    <Header sections={sections} />
  );
}

export default App;
