import { defineConfig } from 'vite';

export default defineConfig({
    root: './',  // Directorio raíz del proyecto Vite
    build: {
        outDir: '../wwwroot',  // Directorio de salida para los archivos compilados
        emptyOutDir: true,  // Limpia la carpeta antes de compilar
        rollupOptions: {
            input: {
                main: './src/js/site.js',
                styles: './src/css/site.css'
            },
            output: {
                // entryFileNames: 'js/[name].js',
                entryFileNames: 'js/site.js',
                assetFileNames: (assetInfo) => {
                    if (assetInfo.name.endsWith('.css')) {
                        // return 'css/[name][extname]';  // Archivos CSS en la carpeta css
                        return 'css/site[extname]';
                    }
                    return 'assets/[name][extname]';  // Otros archivos en la carpeta assets
                }
            }
        }
    },
    server: {
        strictPort: true,
        port: 3000,  // Usa el mismo puerto que ASP.NET Core
        hmr: {
            host: 'localhost'
        }
    }
});