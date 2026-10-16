
type HeaderProps = {
    onToggleSidebar: () => void;
};

function Header({ onToggleSidebar }: HeaderProps) {
    return(
        <header className="flex items-center justify-between rounded-xl bg-surface px-6 py-4 font-sans text-secondary">
            <h1 className="font-display text-2xl font-bold text-secondary">
                ProGear MX
            </h1>

            <button onClick={onToggleSidebar}>
                Menú
            </button>      
        </header>
    );
}

export default Header

