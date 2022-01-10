import React, { Component } from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import TakeoutDiningIcon from '@mui/icons-material/TakeoutDining';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { AuthenticationResultStatus } from './api-authorization/AuthorizeService';
import authService from './api-authorization/AuthorizeService';
import { ApplicationPaths, QueryParameterNames } from './api-authorization/ApiAuthorizationConstants';

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

export default class Login extends Component {
    constructor(props) {
        super(props);

        this.state = {
            message: undefined
        };
    }

    // componentDidMount() {
    //     const action = this.props.action;
    //     switch (action) {
    //         case LoginActions.Login:
    //             this.login(this.getReturnUrl());
    //             break;
    //         case LoginActions.LoginCallback:
    //             this.processLoginCallback();
    //             break;
    //         case LoginActions.LoginFailed:
    //             const params = new URLSearchParams(window.location.search);
    //             const error = params.get(QueryParameterNames.Message);
    //             this.setState({ message: error });
    //             break;
    //         case LoginActions.Profile:
    //             this.redirectToProfile();
    //             break;
    //         case LoginActions.Register:
    //             this.redirectToRegister();
    //             break;
    //         default:
    //             throw new Error(`Invalid action '${action}'`);
    //     }
    // }

    render() {
        return (
            <ThemeProvider theme={theme} >
                <Container component='main' maxWidth='xs'>
                    <CssBaseline />
                    <Box
                        sx={{
                            marginTop: 8,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                        }}
                    >
                        <Avatar sx={{ m: 1, bgcolor: 'primary.main' }}>
                            <TakeoutDiningIcon />
                        </Avatar>
                        <Typography component='h1' variant='h5'>
                            Login
                        </Typography>
                        <Box component='form' onSubmit={this.handleSubmit} noValidate sx={{ mt: 1 }}>
                            {/* Username */}
                            <TextField
                                margin='normal'
                                required
                                fullWidth
                                id='username'
                                label='Username'
                                name='username'
                                type='text'
                                autoFocus />
                            {/* Password */}
                            <TextField
                                margin='normal'
                                required
                                fullWidth
                                name='password'
                                label='Password'
                                type='password'
                                id='password'
                                autoFocus />
                            <FormControlLabel
                                control={<Checkbox value='remember' color='primary' />}
                                label='Remember me' />
                            <Button
                                type="submit"
                                fullWidth
                                variant="contained"
                                sx={{ mt: 3, mb: 2 }}
                            >
                                Login
                            </Button>
                            <Grid container>
                                <Grid item xs>
                                    <Link href="#" variant="body2">
                                        Forgot password?
                                    </Link>
                                </Grid>
                                <Grid item>
                                    <Link href="/signup" variant="body2">
                                        {"Don't have an account? Sign Up"}
                                    </Link>
                                </Grid>
                            </Grid>
                        </Box>
                    </Box>
                    <Copyright sx={{ mt: 8, mb: 4 }} />
                </Container>
            </ThemeProvider>
        );
    }

    handleSubmit(e) {
        e.preventDefault();
        const data = new FormData(e.target);
        // TODO: handle data
    }

    async login(returnUrl) {
        const state = { returnUrl };
        const result = await authService.signIn(state);
        switch (result.status) {
            case AuthenticationResultStatus.Redirect:
                break;
            case AuthenticationResultStatus.Success:
                this.navigateToReturnUrl(returnUrl);
                break;
            case AuthenticationResultStatus.Fail:
                this.setState({ message: result.status });
                break;
            default:
                throw new Error(`Invalid status result ${result.status}.`);
        }
    }

    async processLoginCallback() {
        const url = window.location.href;
        const result = await authService.completeSignIn(url);

        switch (result.status) {
            case AuthenticationResultStatus.Redirect:
                // There should not be any redirects as the only time completeSignIn finishes
                // is when we are doing a redirect sign in flow.
                throw new Error('Should not redirect.');
            case AuthenticationResultStatus.Success:
                await this.navigateToReturnUrl(this.getReturnUrl(result.state));
                break;
            case AuthenticationResultStatus.Fail:
                this.setState({ message: result.message });
                break;
            default:
                throw new Error(`Invalid authentication result status '${result.status}'.`);
        }

    }

    getReturnUrl(state) {
        const params = new URLSearchParams(window.location.search);
        const fromQuery = params.get(QueryParameterNames.ReturnUrl);
        if (fromQuery && !fromQuery.startsWith(`${window.location.origin}/`)) {
            // This is an extra check to prevent open redirects.
            throw new Error("Invalid return url. The return url needs to have the same origin as the current page.")
        }
        return (state && state.returnUrl) || fromQuery || `${window.location.origin}/`;
    }

    redirectToRegister() { // TODO: check
        this.redirectToApiAuthorizationPath(`${ApplicationPaths.IdentityRegisterPath}?${QueryParameterNames.ReturnUrl}=${encodeURI(ApplicationPaths.Login)}`);
    }

    redirectToProfile() {
        this.redirectToApiAuthorizationPath(ApplicationPaths.IdentityManagePath);
    }

    redirectToApiAuthorizationPath(apiAuthorizationPath) {
        const redirectUrl = `${window.location.origin}/${apiAuthorizationPath}`;
        // It's important that we do a replace here so that when the user hits the back arrow on the
        // browser they get sent back to where it was on the app instead of to an endpoint on this
        // component.
        window.location.replace(redirectUrl);
    }

    navigateToReturnUrl(returnUrl) {
        // It's important that we do a replace here so that we remove the callback uri with the
        // fragment containing the tokens from the browser history.
        window.location.replace(returnUrl);
    }
}