import { createTheme, ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import './App.css';
import Header from './components/header/Header';
import { Container } from '@mui/material';

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

const theme = createTheme();

function App() {
    return (
        <ThemeProvider theme={theme}>
			<CssBaseline />
			<Container maxWidth='lg'>
				<Header sections={sections} />
				<main>

				</main>
			</Container>
		</ThemeProvider>

    );
}

export default App;