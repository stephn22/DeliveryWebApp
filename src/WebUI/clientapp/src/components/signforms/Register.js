import React, { Component } from 'react';
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

export default class Register extends Component {
    constructor(props) {
        super(props);

        this.state = {
            text: undefined,
            errorMessage: undefined
        };
    }

    render() {
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
                        <Box component='form' onSubmit={this.handleSubmit} sx={{ mt: 3 }}>
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
                                        onChange={this.validatePersonName}
                                        error={/^[a-zA-Z ]{1,50}$/.test(this.state.text)}
                                        helperText={this.state.errorMessage}
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
                                        onChange={this.validatePersonName}
                                        error={/^[a-zA-Z ]{1,50}$/.test(this.state.text)}
                                        helperText={this.state.errorMessage}
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
                                        onChange={this.validateUsername}
                                        error={/^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$/.test(this.state.text)}
                                        helperText={this.state.errorMessage}
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
                                        onChange={this.validateEmail}
                                        error={/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(this.state.text)}
                                        helperText={this.state.errorMessage}
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
                                        onChange={this.validatePassword}
                                        error={/^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$/.test(this.state.text)}
                                        helperText={this.state.errorMessage}
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
                                        onChange={this.validateConfirmPassword}
                                        error={this.state.text !== ''} // TODO: check
                                        helperText={this.state.errorMessage}
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
                                        <Link href='/login' variant='body2'>
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

    handleSubmit(event) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);
        // TODO: handle data
    }



    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    validatePersonName(e) {
        const text = e.target.value;

        if (/^[a-zA-Z ]{1,50}$/.test(text)) {
            this.setState({ errorMessage: 'Please enter a valid person name' });
        }
    }

    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    validateEmail(e) {
        const text = e.target.value;

        if (/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(text)) {
            this.setState({ errorMessage: 'Please enter a valid email' });
        }
    }

    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    validateUsername(e) {
        const text = e.target.value;

        if (/^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$/.test(text)) {
            this.setState({ errorMessage: 'Please enter a valid username' });
        }
    }

    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    validatePassword(e) {
        const text = e.target.value;

        if (/^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$/.test(text)) {
            this.setState({ errorMessage: 'Please enter a valid password' });
        }
    }

    /**
     * 
     * @param {React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>} e event
     */
    validateConfirmPassword(e) {
        const text = e.target.value;
        const password = ReactDOM.findDOMNode(document.getElementById('password'));

        if (text !== password.nodeValue) {
            this.setState({ errorMessage: 'Passwords do not match' });
        }
    }
}
