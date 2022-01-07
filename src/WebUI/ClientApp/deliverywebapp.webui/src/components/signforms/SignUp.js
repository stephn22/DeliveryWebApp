import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import TakeoutDiningIcon from '@mui/icons-material/TakeoutDining';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import ReactDOM from 'react-dom';

function Copyright(props) {
    return (
        <Typography
            variant='body2'
            color='text.secondary'
            align='center'
            {...props}
        >
            {'Copyright © '}
            <Link color='inherit' href='/'>
                DeliveryApp
            </Link>{' '}
            {new Date().getFullYear()}
            {'.'}
        </Typography>
    );
}

const theme = createTheme();

function SignUp() {

    const handleSubmit = (event) => {
        event.preventDefault();

        const data = new FormData(event.currentTarget);
        // TODO: handle data
    };

    const [text, setText] = React.useState("");
    const [errorMessage, setErrorMessage] = React.useState('');

    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    const validatePersonName = (e) => {
        const text = e.target.value;

        if (/^[a-zA-Z ]{1,50}$/.test(text)) {
            setErrorMessage('Please enter a valid person name');
        }
    };

    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    const validateEmail = (e) => {
        const text = e.target.value;

        if (/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(text)) {
            setErrorMessage('Please enter a valid email');
        }
    };

    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    const validateUsername = (e) => {
        const text = e.target.value;

        if (/^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$/.test(text)) {
            setErrorMessage('Please enter a valid username');
        }
    };

    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    const validatePassword = (e) => {
        const text = e.target.value;

        if (/^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$/.test(text)) {
            setErrorMessage('Please enter a valid password');
        }
    };

    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    const validateConfirmPassword = (e) => {
        const text = e.target.value;
        const password = ReactDOM.findDOMNode(document.getElementById('password'));

        if (text !== password.nodeValue) {
            setErrorMessage('Passwords do not match');
        }
    };

    return (
        <ThemeProvider theme={theme}>
            <Container component='main' maxWidth='xs'>
                <CssBaseline />
                <Box
                    sx={{
                        marginTop: '8',
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center'
                    }}
                >
                    <Avatar sx={{ m: 1, bgcolor: 'primary.main' }}>
                        <TakeoutDiningIcon />
                    </Avatar>
                    <Typography component='h1' variant='h5'>
                        Sign up
                    </Typography>
                    <Box component='form' onSubmit={handleSubmit} sx={{ mt: 3 }}>
                        <Grid container spacing={2}>
                            {/* FName */}
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    required
                                    name='firstName'
                                    fullWidth
                                    id='fname'
                                    label='First Name'
                                    type='text'
                                    variant='outlined'
                                    autoFocus
                                    onChange={validatePersonName}
                                    error={/^[a-zA-Z ]{1,50}$/.test(text)}
                                    helperText={errorMessage}
                                />
                            </Grid>
                            {/* LName */}
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    required
                                    name='lastName'
                                    fullWidth
                                    id='lname'
                                    label='Last Name'
                                    type='text'
                                    variant='outlined'
                                    autoFocus
                                    onChange={validatePersonName}
                                    error={/^[a-zA-Z ]{1,50}$/.test(text)}
                                    helperText={errorMessage}
                                />
                            </Grid>
                            {/* Username */}
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    required
                                    name='username'
                                    fullWidth
                                    id='username'
                                    label='Username'
                                    type='text'
                                    variant='outlined'
                                    autoFocus
                                    onChange={validateUsername}
                                    error={/^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$/.test(text)}
                                    helperText={errorMessage}
                                />
                            </Grid>
                            {/* Email */}
                            <Grid item xs={12}>
                                <TextField
                                    required
                                    name='email'
                                    fullWidth
                                    id='email'
                                    label='Email Address'
                                    type='email'
                                    autoComplete='email'
                                    variant='outlined'
                                    autoFocus
                                    onChange={validateEmail}
                                    error={/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(text)}
                                    helperText={errorMessage}
                                />
                            </Grid>
                            {/* Password */}
                            <Grid item xs={12}>
                                <TextField
                                    required
                                    name='password'
                                    fullWidth
                                    id='password'
                                    label='Password'
                                    type='password'
                                    variant='outlined'
                                    onChange={validatePassword}
                                    error={/^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$/.test(text)}
                                    helperText={errorMessage}
                                />
                            </Grid>
                            {/* Confirm Password */}
                            <Grid item xs={12}>
                                <TextField
                                    required
                                    name='confirmPassword'
                                    fullWidth
                                    id='confirmPassword'
                                    label='Confirm Password'
                                    type='password'
                                    variant='outlined'
                                    onChange={validateConfirmPassword}
                                    error={text !== ''} // TODO: check
                                    helperText={errorMessage}
                                />
                            </Grid>
                            <Button
                                type='submit'
                                fullWidth
                                variant='contained'
                                sx={{ mt: 3, mb: 2 }}
                            >
                                Sign Up
                            </Button>
                            <Grid container justifyContent='flex-end'>
                                <Grid item>
                                    <Link href='#' variant='body2'>
                                        Already have an account? Sign in
                                    </Link>
                                </Grid>
                            </Grid>
                        </Grid>
                    </Box>
                </Box>
                <Copyright sx={{ mt: 5 }} />
            </Container>
        </ThemeProvider>
    );
}

export default SignUp;
